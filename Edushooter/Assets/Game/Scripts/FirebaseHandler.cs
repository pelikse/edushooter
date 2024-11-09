using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Auth;
using Firebase.Functions;

using MoreMountains.Tools;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using MoreMountains.TopDownEngine;
using System.Net;



public class FirebaseHandler : MMPersistentSingleton<FirebaseHandler>
{
    private static readonly string UPDATE_GEMCOUNT_URL = "https://updategemcount-tfxf5ks7wa-de.a.run.app";
    private static readonly string CREATE_NEW_EMAIL_URL = "https://createnewemailpassworduser-tfxf5ks7wa-de.a.run.app";

    public static FirebaseHandler TryGetInstance()
    {
        return Current;
    }

    #region Debugging Buttons
    [MMInspectorButton("SignOutAuth")]
    /// A test button to sign out from the inspector
    public bool DebugSignOut;
    protected virtual void SignOutAuth()
    {
        SignOut();
    }

    [MMInspectorButton("GetUserAuth")]
    /// A test button to get current authenticated data
    public bool DebugGetUser;
    protected virtual void GetUserAuth()
    {
        Debug.Log("user email: " + user.Email);
        Debug.Log("id: " + user.UserId);
    }

    #endregion

    #region Class Definitions
    [System.Serializable]
    public class UserSerializable
    {
        public string UserID;
        public string DisplayName;
    }

    [System.Serializable]
    public class FirebaseTimestamp
    {
        public int _seconds;
    }

    [System.Serializable]
    public class UserDataWrapper
    {
        public UserDataSchema data;
    }

    [System.Serializable]
    [FirestoreData]
    public class UserDataSchema
    {
        [FirestoreProperty]
        public string displayName { get; set; }

        [FirestoreProperty]
        public string[] friendList { get; set; }

        [FirestoreProperty]
        public string userID { get; set; }

        [FirestoreProperty]
        public Timestamp creationTime { get; set; }

        [FirestoreProperty]
        public int gemCount { get; set; }
    }

    public class RequestPayload
    {
        public string token;
    }

    public class RequestUpdateGems : RequestPayload
    {
        public int amount;
        public bool addGems;
    }

    public class RequestCreateEmailPasswordAccount : RequestPayload
    {
        public string email;
        public string password;
    }

    #endregion

    #region FirestoreCloudSchema

    [FirestoreData]
    public class EdushooterFirestoreData
    {
        [FirestoreProperty]
        public int coinCount { get; set; }

        [FirestoreProperty]
        public CharacterAbilityData abilityData { get; set; }

        [FirestoreProperty]
        public CharacterEquipmentData equipmentData { get; set; }

        [FirestoreProperty]
        public List<EdushooterResults> bestSessions { get; set; }

        [FirestoreProperty]
        public Timestamp lastLogin { get; set; }

        [FirestoreProperty]
        public int loginStreak { get; set; }

        [FirestoreProperty]
        public bool hasLoggedIn { get; set; }
    }

    [FirestoreData]
    public class CharacterAbilityData
    {
        [FirestoreProperty]
        public short HealthLevel { get; set; }

        [FirestoreProperty]
        public short SpeedLevel { get; set; }

        [FirestoreProperty]
        public short DamageLevel { get; set; }

        [FirestoreProperty]
        public short DashLevel { get; set; }

        [FirestoreProperty]
        public short ReloadLevel { get; set; }
    }

    [FirestoreData]
    public class CharacterEquipmentData
    {
        [FirestoreProperty]
        public List<WeaponType> UnlockedWeapons { get; set; }

        [FirestoreProperty]
        public List<ModelType> UnlockedHeadSkins { get; set; }

        [FirestoreProperty]
        public List<ModelType> UnlockedBodySkins { get; set; }
    }

    [FirestoreData]
    public class CharacterLeaderboardData
    {
        [FirestoreProperty]
        public Timestamp FinishTime { get; set; }

        [FirestoreProperty]
        public MapType PlayedMap { get; set; }

        [FirestoreProperty]
        public short DifficultyLevel { get; set; }

        [FirestoreProperty]
        public int SessionTime { get; set; }
    }

    public void UploadCloudData(PlayerExternalData data)
    {
        if (!IsSignedIn())
        {
            Debug.LogError("player is not signed in, cannot upload data!");
            return;
        }

        if (!Initialized)
        {
            Debug.LogError("Firebase is not initialized, cannot upload data!");
            return;
        }

        // Create the cloud firestore schema
        EdushooterFirestoreData firestoreData = new EdushooterFirestoreData()
        {
            coinCount = data.Coins,

            abilityData = new CharacterAbilityData()
            {
                HealthLevel = data.EdushooterStats.Health,
                SpeedLevel = data.EdushooterStats.Speed,
                DamageLevel = data.EdushooterStats.Damage,
                DashLevel = data.EdushooterStats.Dash,
                ReloadLevel = data.EdushooterStats.Reload,
            },

            equipmentData = new CharacterEquipmentData()
            {
                UnlockedWeapons = data.EdushooterStats.UnlockedWeapons,
                UnlockedBodySkins = data.PlayerUnlockables.unlockedBodies,
                UnlockedHeadSkins = data.PlayerUnlockables.unlockedHeads,
            },

            bestSessions = data.BestSessions,

            lastLogin = data.LastLoginTime,

            loginStreak = data.LoginStreak,

            hasLoggedIn = data.HasLoggedInToday,
        };

        // Upload it to the appropriate collection
        DocumentReference playerData = db.Collection(USER_DATA_COLLECTION).Document(user.UserId)
                                        .Collection(INDEPENDENT_GAME_DATA).Document(EDUSHOOTER_CLOUD_DATA);

        try
        {
            playerData.SetAsync(firestoreData, SetOptions.MergeAll).ContinueWith((task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Something went wrong when syncing cloud data: " + task.Exception.Message);
                    throw new Exception(task.Exception.Message);
                }

                Debug.Log("Uploaded user data to firestore!");
            }); //this will merge the updated data with the old one if necessary
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    public async Task<EdushooterFirestoreData> FetchCloudDataAsync()
    {
        if (!IsSignedIn())
        {
            Debug.LogError("Player is not signed in, cannot fetch data!");
            return null;
        }

        if (!Initialized)
        {
            Debug.LogError("Firebase is not initialized, cannot fetch data!");
            return null;
        }

        // Get a reference to the stored data
        DocumentReference cloudData = db.Collection(USER_DATA_COLLECTION).Document(user.UserId)
                                        .Collection(INDEPENDENT_GAME_DATA).Document(EDUSHOOTER_CLOUD_DATA);

        try
        {
            // Get a snapshot
            DocumentSnapshot snapshot = await cloudData.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                EdushooterFirestoreData fetchedData = snapshot.ConvertTo<EdushooterFirestoreData>();

                if (fetchedData == null)
                {
                    Debug.LogError("The user's cloud data is empty, it needs to be initialized first!");
                }

                Debug.Log("Returning fetched cloud data");
                return fetchedData;
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching data: {ex.Message}");
            return null;
        }
    }


    #endregion

    public UnityEvent OnInitialized;

    [MMInformation("This script should always be put on an object that is present in the LANDING PAGE! This must be initialized for Firebase to run properly!", MMInformationAttribute.InformationType.Warning, true)]
    public UnityEvent OnFailedInitialization;

    public bool InitializeOnStart = false;

    [Space]

    public UserDataSchema userData;

    [Space, Space]

    // cache
    [SerializeField]
    [MMReadOnly]
    private bool Initialized = false;


    private FirebaseApp app;
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFunctions functions;

    private bool _signedIn = false;
    private bool _fetchingUserData = false;

    #region Firestore Constants

    const string FIRESTORE_DATABASE = "edushooter-development";

    const string USER_DATA_COLLECTION = "users";

    const string INDEPENDENT_GAME_DATA = "game_data";
    const string EDUSHOOTER_CLOUD_DATA = "edushooter-data";

    const string EDUSHOOTER_DATA_COLLECTION = "edushooter_sessions";
    const string PURCHASE_UPGRADE_COLLECTION = "purchase_upgrade";
    const string PURCHASE_WEAPON_COLLECTION = "purchase_weapon";
    const string PURCHASE_SKIN_COLLECTION = "purchase_skin";

    public UserDataSchema UserData { get => userData; set => userData = value; }

    #endregion

    // initializes firebase
    public bool FirebaseIsInitialized()
    {
        if (!Initialized)
        {
            Debug.LogError("Firebase not initialized yet!");
        }

        return Initialized;
    }

    // tries and gets the player ID through the user class, if its unavailable then it returns ""
    public string TryGetPlayerID()
    {
        if (user != null)
        {
            return user.UserId;
        }

        Debug.Log("no user detected.. returning null");
        return "";
    }

    // get the firestore instance or creates one
    public FirebaseFirestore GetFirestore()
    {
        if (Initialized) return FirebaseFirestore.GetInstance("edushooter-development");
        else return null;
    }

    // get the ID of the player
    private string GetPlayerDocumentID()
    {
        string id = ProgressManager.TryGetInstance().GetPlayerID();
        if (id == null)
        {
            id = TryGetPlayerID();
            ProgressManager.TryGetInstance().SetPlayerID(id);
        }

        return id;
    }

    // get the player's display name
    public string GetPlayerDisplayName()
    {
        if (!Initialized)
        {
            Debug.LogError("firebase not initialized yet! user cannot be accessed");
            return null;
        }
        
        return user.DisplayName;
    }

    #region Initialization
    public void InitializeFirebase()
    {
        if (app == null)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeApp();
                    InitializeFirestore();
                    InitializeAuthentication();
                    InitializeFunctions();
                    InitializeUserData();

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    Initialized = true;

                    Debug.Log("Firebase initialized and ready to use!");
                    OnInitialized?.Invoke();

                    // If we're signed in then load save
                    if (IsSignedIn())
                    {
                        SyncSharedUserData();
                        ProgressManager.TryGetInstance().LoadSave();
                        MMGameEvent.Trigger("TriggerSave");
                    }
                }
                else
                {
                    Debug.LogError(string.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));

                    OnFailedInitialization?.Invoke();
                }
            });
        }

    }

    private void InitializeApp()
    {
        // create and hold a firebase app instance
        app = FirebaseApp.DefaultInstance;
    }

    private void InitializeFirestore()
    {
        // get the firestore of the development database
        db = FirebaseFirestore.GetInstance(FIRESTORE_DATABASE);
    }

    private void InitializeAuthentication()
    {
        // get the default authentication instance
        auth = FirebaseAuth.DefaultInstance;

        // attach an event listener
        auth.StateChanged += AuthenticationListener;
        AuthenticationListener(this, null);
    }

    private void InitializeFunctions()
    {
        functions = FirebaseFunctions.GetInstance(app ?? FirebaseApp.DefaultInstance, "asia-east1");
    }

    private void InitializeUserData()
    {
        SyncSharedUserData();
    }

    #endregion

    #region Authentication

    public bool IsSignedIn()
    {
        return _signedIn;
    }
    public void SignOut(Action signOutAction = null)
    {
        if (_signedIn)
        {
            auth.SignOut();
            signOutAction?.Invoke();
            // reset the local cache
            ProgressManager.TryGetInstance().ResetCache();
            ProgressManager.TryGetInstance().ResetSaveData();
        }
        else
        {
            Debug.LogError("player is already signed out!");
        }
    }

    // Track state changes of the auth object.
    void AuthenticationListener(object sender, EventArgs eventArgs)
    {
        // if the current user isnt authenticated
        if (auth.CurrentUser != user)
        {
            //check for signed in state
            _signedIn = (user != auth.CurrentUser) && (auth.CurrentUser != null) && (auth.CurrentUser.IsValid());

            //if we're not signed but there's a user, then we've signed out
            if (!_signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                ProgressManager.TryGetInstance()?.ResetCache();
            }

            //else, we'll sign in the user
            user = auth.CurrentUser;
            if (_signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                Debug.Log("User data:\n" + user.DisplayName + '\n' + user.PhotoUrl);

                // If we're signed in and firebase is initialized, then load save
                if (FirebaseIsInitialized())
                {
                    SyncSharedUserData();
                    ProgressManager.TryGetInstance().LoadSave();
                }
            }
        }
    }

    public async Task<bool> SignUpUser(string email, string password, Action successCallback = null)
    {
        if (!Initialized) return false;

        bool _success = false;
        string _errorMsg = "Sign In Canceled";

        //try and register the email or password
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                //notify that something wrong happened to the user
                _errorMsg = task.Exception.Message;
                return;
            }

            // firebase user has been created
            FirebaseUser currentUser = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", currentUser.DisplayName, currentUser.UserId);
            _success = true;

            successCallback?.Invoke();
        });

        if (!_success)
        {
            PopupElement.TryGetInstance().DisplayPopup("Sign Up Failed!", "<size=-10>" + _errorMsg + "</size>\n\nPlease try again!");
        }

        return _success;
    }

    public async Task<bool> SignInUser(string email, string password, Action resultCallback = null)
    {
        if (!Initialized) return false;

        bool _success = false;
        string _errorMsg = "Sign In Canceled";

        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                _errorMsg = task.Exception.Message;
                return;
            }

            _success = true;

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });

        if (!_success)
        {
            PopupElement.TryGetInstance().DisplayPopup("Sign In Failed!", "<size=-10>" + _errorMsg + "</size>\n\nPlease try again!");
        }

        return _success;
    }

    public async Task<bool> LinkUserEmailCredentials(string email, string password, Action successCallback = null)
    {
        if (!Initialized) return false;

        bool _success = false;
        string _errorMsg = "Internal Server Error!";

        //// create a token and a json payload
        //string newToken = await user.TokenAsync(true);

        //RequestCreateEmailPasswordAccount payload = new RequestCreateEmailPasswordAccount
        //{
        //    token = newToken,
        //    email = email,
        //    password = password
        //};

        //string requestPayload = JsonUtility.ToJson(payload);

        ////await for the completion of the new account
        //bool newAccountCreated = await CreateNewEmailPasswordAccount(requestPayload);
        
        //if new account is successfully created

        Credential newCredential = EmailAuthProvider.GetCredential(email, password);

        //attempt to link the current user with the new user
        await user.LinkWithCredentialAsync(newCredential).ContinueWith((task) => {
            if (task.IsCanceled)
            {
                Debug.LogError("LinkWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
                _errorMsg = task.Exception.Message;
                return;
            }

            _success = true;

            AuthResult result = task.Result;
            Debug.LogFormat("User linked with successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
        

        //if linking failed
        if (!_success)
        {
            PopupElement.TryGetInstance()?.DisplayPopup("Failure to Link!",_errorMsg);
        }

        return _success;
    }

    public async Task<bool> LinkCredentialsToCurrentUser(Credential newCredential)
    {
        if (!Initialized) return false;
        bool _success = false;

        //try and register the email or password
        await user.LinkWithCredentialAsync(newCredential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("LinkWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            //successfully linked
            _success = true;
        });

        return _success;
    }

    public async Task<bool> SignInAnonymously(Action successCallback = null)
    {
        if (!Initialized) return false;

        bool _success = false;
        string _errorMsg = "Sign In Canceled";

        await auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                _errorMsg = task.Exception.Message;
                return;
            }

            _success = true;

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });

        if (!_success)
        {
            PopupElement.TryGetInstance().DisplayPopup("Sign In Failed!", "<size=-10>" + _errorMsg + "</size>\n\nPlease try again!");
        }

        return _success;
    }

    public async Task<bool> SignInWithGooglePlay(Credential googleCredentials)
    {
        if (!Initialized) return false;

        bool _success = false;
        string _errorMsg = "Sign In Canceled";

        await auth.SignInWithCredentialAsync(googleCredentials).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                _errorMsg = task.Exception.Message;
                return;
            }

            _success = true;

            FirebaseUser result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.DisplayName, result.UserId);
        });

        if (!_success)
        {
            PopupElement.TryGetInstance().DisplayPopup("Sign In Failed!", "<size=-10>" + _errorMsg + "</size>\n\nPlease try again!");
        }

        return _success;
    }

    #endregion

    #region Set Datafields

    // this function creates a firestore document that denotes any errors during the app runtime relating to
    // firebase uploads
    private async void UploadError(string errorMsg, string id)
    {
        // contact firebase to create a document about an error
        FirebaseFirestore db = GetFirestore();
        if (db != null)
        {
            DocumentReference doc = db.Collection("Errors").Document();
            Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            //create the basic user document
            Dictionary<string, object> userInfo = new Dictionary<string, object>
            {
                { "PlayerId",  id},
                { "ErrorTime", currentTimestamp },
                { "ErrorMessage", errorMsg }
            };

            try
            {
                await doc.SetAsync(userInfo).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added new player document in firestore");
                });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public async void SetCharacterName(string name)
    {
        if (!Initialized) return;

        //check if the player has an id already
        if (!ProgressManager.TryGetInstance().GetPlayerID().Equals(""))
        {
            if (user != null)
            {
                UserProfile profile = new UserProfile()
                {
                    DisplayName = name,
                };

                await user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log("User profile updated successfully.");
                });

                // Upload it to the appropriate collection
                DocumentReference userEntry = db.Collection(USER_DATA_COLLECTION).Document(user.UserId);

                Dictionary<string, object> updateName = new Dictionary<string, object>
                {
                    { "displayName", name }
                };
                await userEntry.SetAsync(updateName, SetOptions.MergeAll);

                Debug.Log("finished updating current username");
            }
        }
        else Debug.LogError("trying to set the character name without a player ID!");
    }

    public async void UploadEdushooterResults(bool victory, float duration, int difficultyLevel, int coinReward, int enemyKilled, MapType map)
    {
        Debug.Log("uploading edushooter data");

        if (!FirebaseIsInitialized()) return;

        FirebaseFirestore db = GetFirestore();

        if (db != null)
        {
            //get the player ID, name, and current time
            string id = GetPlayerDocumentID();
            string name = ProgressManager.TryGetInstance().GetDisplayName();
            Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            // make a reference to a new document 
            DocumentReference sessionDoc = db.Collection(EDUSHOOTER_DATA_COLLECTION).Document();

            //create the edushooter document
            Dictionary<string, object> sessionInfo = new Dictionary<string, object>
            {
                { "PlayerID", id },                     // the id of the player
                { "PlayerName", name },                 // the name of the player
                { "FinishTime", currentTimestamp },     // the time that the session finished
                { "MapType", map.ToString() },          // the map
                { "Victory", victory },                  // victory or loss
                { "EnemyKilled", enemyKilled },         // the number of enemies killed
                { "GameDuration", duration },           // how long the game lasted
                { "DifficultyLevel", difficultyLevel }, // the difficulty level
                { "CoinReward", coinReward },           // the coin rewarded
            };

            try
            {
                await sessionDoc.SetAsync(sessionInfo).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added new edushooter session document in firestore");
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during edushooter session upload: " + ex.Message);
                UploadError(ex.Message, id);
            }
        }
        else
        {
            Debug.LogWarning("database is null!!");
        }
    }

    public async void UploadUpgradePurchase(int cost, CharacterUpgradeManager.Stat upgradedStat, int upgradedLevel)
    {
        Debug.Log("uploading upgrade data to firestore");

        if (!FirebaseIsInitialized()) return;

        FirebaseFirestore db = GetFirestore();

        if (db != null)
        {
            //get the player ID and current time
            string id = GetPlayerDocumentID();
            Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            // make a reference to a new document 
            DocumentReference purchaseDoc = db.Collection(PURCHASE_UPGRADE_COLLECTION).Document();

            //create the purchase document
            Dictionary<string, object> purchaseInfo = new Dictionary<string, object>
        {
            { "PlayerID", id },
            { "PurchaseTime", currentTimestamp },
            { "UpgradedStat", upgradedStat.ToString() },
            { "PreviousLevel", upgradedLevel-1 },
            { "UpgradedLevel", upgradedLevel },
            { "UpgradeCost", cost },
        };

            try
            {
                await purchaseDoc.SetAsync(purchaseInfo).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added new upgrade occassion document in firestore");
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during purchase session upload: " + ex.Message);
                UploadError(ex.Message, id);
            }
        }
    }

    public async void UploadWeaponPurchase(int cost, WeaponType weapon)
    {
        Debug.Log("uploading weapon purchase data to firestore");

        if (!FirebaseIsInitialized()) return;

        FirebaseFirestore db = GetFirestore();

        if (db != null)
        {
            //get the player ID and current time
            string id = GetPlayerDocumentID();
            Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            // make a reference to a new document 
            DocumentReference purchaseDoc = db.Collection(PURCHASE_WEAPON_COLLECTION).Document();

            //create the purchase document
            Dictionary<string, object> purchaseInfo = new Dictionary<string, object>
            {
                { "PlayerID", id },
                { "PurchaseTime", currentTimestamp },
                { "WeaponPurchase", weapon.ToString() },
                { "UpgradeCost", cost },
            };

            try
            {
                await purchaseDoc.SetAsync(purchaseInfo).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added new weapon purchasing occassion document in firestore");
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during weapon session upload: " + ex.Message);
                UploadError(ex.Message, id);
            }
        }
    }

    public async void UploadSkinPurchase(int cost, ModelType model, SkinSelector.SkinType type)
    {
        Debug.Log("uploading skin purchase data to firestore");

        if (!FirebaseIsInitialized()) return;

        FirebaseFirestore db = GetFirestore();

        if (db != null)
        {
            //get the player ID and current time
            string id = GetPlayerDocumentID();
            Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            // make a reference to a new document 
            DocumentReference purchaseDoc = db.Collection(PURCHASE_SKIN_COLLECTION).Document();

            //create the purchase document
            Dictionary<string, object> purchaseInfo = new Dictionary<string, object>
            {
                { "PlayerID", id },
                { "PurchaseTime", currentTimestamp },
                { "SkinPurchased", model.ToString() },
                { "SkinType", type.ToString() },
                { "SkinCost", cost },
            };

            try
            {
                await purchaseDoc.SetAsync(purchaseInfo).ContinueWithOnMainThread(task =>
                {
                    Debug.Log("Added new weapon purchasing occassion document in firestore");
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception during weapon session upload: " + ex.Message);
                UploadError(ex.Message, id);
            }
        }
    }

    #endregion

    #region Eduproject API Calls

    public async void SyncSharedUserData()
    {
        if (!Initialized || user == null)
        {
            Debug.LogWarning("firebase uninitialized");
            return;
        }

        if (_fetchingUserData)
        {
            Debug.LogError("attempting to fetch user data while fetching is already in progress!");
            return;
        }
        else
        {
            _fetchingUserData = true;
        }
        Debug.Log("fetching shared user data!");

        // Get a reference to the stored data
        DocumentReference cloudData = db.Collection(USER_DATA_COLLECTION).Document(user.UserId);

        try
        {
            // Get a snapshot
            DocumentSnapshot snapshot = await cloudData.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Debug.Log(string.Format("Shared data for {0} fetched", snapshot.Id));
                UserDataSchema fetchedData = snapshot.ConvertTo<UserDataSchema>();
                Debug.Log(string.Format("{0} {1} {2}", fetchedData.userID, fetchedData.creationTime, fetchedData.gemCount));

                if (fetchedData != null)
                {
                    userData = fetchedData;
                    ProgressManager.TryGetInstance().SetGemAmountNoSync(userData.gemCount);
                }
                else
                {
                    Debug.LogError("The user's cloud data is empty, it needs to be initialized first!");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching data: {ex.Message}");
        }

        _fetchingUserData = false;
    }

    public async void UpdateUserGemCount(int gems = 0, bool addGems = true)
    {
        if (!Initialized || user == null)
        {
            Debug.LogWarning("firebase uninitialized");
            return;
        }

        Debug.Log("attempting to update gem amount by " + (addGems ? "+" : "-") + gems + "...");
        string newToken = await user.TokenAsync(true);

        Debug.Log("token is " + newToken);
        if (newToken != null)
        {
            // Update the local data first
            userData.gemCount = Mathf.Clamp((userData.gemCount + (addGems ? 1 : -1) * gems), 0, ProgressManager.GEM_CAP);

            // Call the user data API using the token
            RequestUpdateGems payload = new RequestUpdateGems
            {
                token = newToken,
                amount = gems,
                addGems = addGems
            };

            string requestPayload = JsonUtility.ToJson(payload);

            StartCoroutine(UpdateGemCount(requestPayload, UPDATE_GEMCOUNT_URL));
        }
        else
        {
            //failure in getting token, handle it
            Debug.LogError("error in getting authentication token!");
        }
    }

    private IEnumerator UpdateGemCount(string jsonData, string url)
    {
        UnityWebRequest req = PostFormatter(jsonData, url);

        //wait for a response
        yield return req.SendWebRequest();

        //check for errors
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
            Debug.LogError(req.downloadHandler.text);
        }
        else
        {
            // Process the response
            string response = req.downloadHandler.text;
            Debug.Log(response);

            Debug.Log("finished updating gem data");
        }
    }

    private async Task<bool> CreateNewEmailPasswordAccount(string jsonData)
    {
        UnityWebRequest req = PostFormatter(jsonData, CREATE_NEW_EMAIL_URL);

        var operation = req.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();  // Wait until the request is done without blocking the main thread
        }

        // Check for errors
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
            Debug.LogError(req.downloadHandler.text);

            return false;
        }
        else
        {
            // Process the response
            string response = req.downloadHandler.text;
            Debug.Log(response);

            if (req.responseCode == 200)
            {
                Debug.Log("Finished creating new account");
                return true;
            }

            return false;
        }
    }

    private IEnumerator FetchUserData(string jsonData, string url)
    {
        UnityWebRequest req = PostFormatter(jsonData, url);

        //wait for a response
        yield return req.SendWebRequest();

        //check for errors
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error);
            Debug.LogError(req.downloadHandler.text);
        }
        else
        {
            // Process the response
            string response = req.downloadHandler.text;
            Debug.Log(response);

            if (req.responseCode == 200)
            {
                //successful fetch, assign to local data
                userData = JsonUtility.FromJson<UserDataWrapper>(response).data;
                ProgressManager.TryGetInstance().SetGemAmountNoSync(userData.gemCount);

                Debug.Log("finished updating gem data");
            }
        }
    }

    private UnityWebRequest PostFormatter(string jsonData, string url)
    {
        //create and format the request
        UnityWebRequest req = new UnityWebRequest(url, "POST");
        byte[] payload = new System.Text.UTF8Encoding().GetBytes(jsonData);

        req.uploadHandler = new UploadHandlerRaw(payload);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        return req;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (InitializeOnStart)
        {
            InitializeFirebase();
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthenticationListener;
            auth = null;
        }
    }
}
