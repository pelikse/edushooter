using Firebase.Firestore;
using MoreMountains.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

#region Data Structures

//the types of skin available
[System.Serializable]
public enum ModelType
{
    BlondBoy = 0,
    BlueGirl = 1,
    Alien = 2,
    TeddyBear = 3,
    Ghillie = 4,
    Chicken = 5,
    Jester = 6,
    Paperbag = 7,
    Hoodie = 8,
    Cowboy = 9,
    Guerilla = 10,
    Soldier = 11,
    Criminal = 12,
    Chemist = 13,
    Knight = 14,
    Hero = 15,
    Android = 16,
    Samurai = 17,
    Ninja = 18,
    Astronaut = 19,
}

[System.Serializable]
public enum WeaponType
{
    AssaultRifle = 0,
    Shotgun = 1,
    Sniper = 2,
    Smg = 3,
    Gatling = 4,
    Rpg = 5,
    Flamethrower = 6,
}

[System.Serializable]
public enum MapType
{
    Neighborhood,
    Meadows,
    Warehouse,
    Supermarket,
    School,
    Plaza,
    Park,
    Airport,
}

[System.Serializable]
public class PlayerExternalData
{
    public string PlayerID = "";
    public string DisplayName = "Default Danny";

    [Space]

    public int Coins = 0;
    public int Gems = 0;

    [Space]

    public PlayerEdushooterData EdushooterStats;

    public List<EdushooterResults> BestSessions;

    [Space]

    public PlayerUnlockables PlayerUnlockables;

    [Space]

    public Timestamp LastLoginTime;
    public int LoginStreak;
    public bool HasLoggedInToday;
}

[System.Serializable]
public class PlayerEdushooterData
{
    //levels of player stats
    [SerializeField] private short health = 0;
    [SerializeField] private short damage = 0;
    [SerializeField] private short speed = 0;
    [SerializeField] private short dash = 0;
    [SerializeField] private short reload = 0;

    //unlocked weapons
    [SerializeField] private List<WeaponType> unlockedWeapons = new List<WeaponType>();

    //encapsulation
    public short Health { get => health; set => health = value; }
    public short Damage { get => damage; set => damage = value; }
    public short Speed { get => speed; set => speed = value; }
    public short Dash { get => dash; set => dash = value; }
    public short Reload { get => reload; set => reload = value; }

    public List<WeaponType> UnlockedWeapons { get => unlockedWeapons; set => unlockedWeapons = value; }

}

[System.Serializable]
public class PlayerUnlockables
{
    //the way unlockables are stored is with an array of booleans, with each enum corresponding to its own element inside the array
    public List<ModelType> unlockedHeads;
    public List<ModelType> unlockedBodies;
}

[System.Serializable]
[FirestoreData]
public class EdushooterResults
{
    [FirestoreProperty]
    public int DifficultyLevel { get; set; }
    [FirestoreProperty]
    public MapType Map { get; set; }
    [FirestoreProperty]
    public Timestamp LastFinishTime { get; set; }
    [FirestoreProperty]
    public int TimeToFinish { get; set; }
}

[System.Serializable]
public class LocalEdushooterStorage
{
    [SerializeField] private WeaponType equippedWeapon;

    [SerializeField] private bool shuffledMapPlayed = false;

    [SerializeField] private MapType selectedMap;

    [SerializeField] private MapType availableMapA;
    [SerializeField] private MapType availableMapB;

    [SerializeField] private ModelType bodyModel;
    [SerializeField] private ModelType headModel;

    //last difficulty level of edushooter
    [SerializeField] private short difficulty;

    // Encapsulation
    public ModelType BodyModel { get => bodyModel; set => bodyModel = value; }
    public ModelType HeadModel { get => headModel; set => headModel = value; }

    public MapType AvailableMapA { get => availableMapA; set => availableMapA = value; }
    public MapType AvailableMapB { get => availableMapB; set => availableMapB = value; }
    public bool ShuffledMapPlayed { get => shuffledMapPlayed; set => shuffledMapPlayed = value; }
    public WeaponType EquippedWeapon { get => equippedWeapon; set => equippedWeapon = value; }
    public MapType SelectedMap { get => selectedMap; set => selectedMap = value; }
    public short DifficultyLevel { get => difficulty; set => difficulty = value; }
}

#endregion

public class ProgressManager : MMPersistentSingleton<ProgressManager>, MMEventListener<MMGameEvent>
{
    public bool LoadSaveOnStart = false;
    public bool CheckForFirebaseOnLoad = true;

    #region Debugging Buttons
    [MMInspectorButton("TestAddCoins")]
    /// A test button to test adding coins
    public bool TestAddCoinsBtn;
    protected virtual void TestAddCoins()
    {
        AddCoins(50);
    }

    [MMInspectorButton("TestAddGems")]
    /// A test button to test adding coins
    public bool TestAddGemsBtn;
    protected virtual void TestAddGems()
    {
        AddGems(60);
    }

    [MMInspectorButton("SaveGameFunc")]
    /// A test button to test creating the save file
    public bool SaveGameFuncBtn;
    protected virtual void SaveGameFunc()
    {
        SaveGame();
    }

    [MMInspectorButton("LoadGameFunc")]
    /// A test button to test creating the save file
    public bool LoadGameFuncBtn;
    protected virtual void LoadGameFunc()
    {
        LoadSave();
    }

    [MMInspectorButton("UploadPlayerDataFunc")]
    /// A test button to test creating the save file
    public bool UploadPlayerDataBtn;
    protected virtual void UploadPlayerDataFunc()
    {
        UploadPlayerData();
    }

    #endregion

    // Attributes

    public PlayerExternalData ExternalData;
    public LocalEdushooterStorage LocalCache;

    public short LevelCap = 10;

    protected const string _saveFolderName = "EdugameProgressData";
    protected const string _saveFileName = "EdugameProgress.data";

    protected const string _cacheFolderName = "LocalEdushooterCache";
    protected const string _cacheFileName = "LocalCache.data";

    public static int COIN_CAP = 999999;
    public static int GEM_CAP = 9999;


    protected const int BASE_GEM_REWARD = 15;

    protected const float LOGIN_COOLDOWN_DURATION_HOURS = 20f;
    protected const float LOGIN_EXPIRED_THRESHOLD_HOURS = 48f;
    protected const ushort LOGIN_STREAK_DURATION = 7;

    protected const ModelType STARTING_MODEL = ModelType.Guerilla;

    protected bool _isLoading = false;

    /// <summary>
    /// Statics initialization to support enter play modes
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    protected static void InitializeStatics()
    {
        _instance = null;
    }
    public static int CalculateGemReward(int difficulty)
    {
        //for every difficulty level, add 2 to the reward
        return BASE_GEM_REWARD + difficulty * 5;
    }

    //on awake load progress
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

        if (LoadSaveOnStart)
        {
            if (CheckForFirebaseOnLoad)
            {
                if (!FirebaseHandler.TryGetInstance().FirebaseIsInitialized())
                {
                    return;
                }
            }

            LoadSave();
        }
    }


    #region User Data Management
    public async virtual void LoadSave()
    {
        Debug.Log("attempting to load data from firestore...");

        // if the firebase handler isnt initialized
        if (!FirebaseHandler.TryGetInstance().FirebaseIsInitialized())
        {
            Debug.LogError("trying to load while firebase is uninitialized!");
            return;
        }

        // set the flag to loading
        // guard code so two load operations dont happen simultaneously
        if (_isLoading)
        {
            Debug.LogError("trying to load while loading is in progress!");
            return;
        }
        else
        {
            LoadingOverlayManager.TryGetInstance().AddBlockingProcess();
            _isLoading = true;
        }

        // Gets data from the cloud and then loads the data
        PlayerExternalData savedata = LoadCloudData(await FirebaseHandler.TryGetInstance().FetchCloudDataAsync());

        if (savedata != null)
        {
            Debug.Log("cloud data synced with local data!");
            ExternalData = savedata;

            //no id
            if (ExternalData.PlayerID == "")
            {
                Debug.Log("no id detected");
                //get id
                SetPlayerID(FirebaseHandler.TryGetInstance().TryGetPlayerID());
            }

            double _sinceLastLogin = MiscellaneousMethods.TimeSinceTimestamp(savedata.LastLoginTime);

            //if the time since last login is beyond the cooldown
            if ( _sinceLastLogin > LOGIN_COOLDOWN_DURATION_HOURS)
            {
                Debug.Log("time since last login is " + MiscellaneousMethods.TimeSinceTimestamp(savedata.LastLoginTime));
                //if the login streak has expired
                if (_sinceLastLogin > LOGIN_EXPIRED_THRESHOLD_HOURS)
                {
                    savedata.LoginStreak = 0;
                    Debug.Log("Player has failed their login streak, resetting to 0!");
                }
                else
                {
                    savedata.LoginStreak = (savedata.LoginStreak + 1) % LOGIN_STREAK_DURATION;
                    Debug.Log("Player logged in, preserving login streak! Streak is now " + savedata.LoginStreak);
                }

                //set the last login time to now
                savedata.LastLoginTime = Timestamp.GetCurrentTimestamp();
            }
            else
            {
                Debug.Log("player's login reward is still on cooldown. there are " + (LOGIN_COOLDOWN_DURATION_HOURS-_sinceLastLogin) + " hours left.");
            }
        }
        //no save data yet, assume clean state
        else
        {
            Debug.Log("no save data detected, creating new save");
            CreateDefaultSaveAsync();
        }

        LocalEdushooterStorage localdata = (LocalEdushooterStorage)MMSaveLoadManager.Load(typeof(LocalEdushooterStorage), _cacheFileName, _cacheFolderName);
        if (localdata != null)
        {
            Debug.Log("loaded cache");
            LocalCache = localdata;
        }
        else
        {
            Debug.Log("no local cache detected, creating new cache");
            CreateNewCache();
        }

        _isLoading = false;
        LoadingOverlayManager.TryGetInstance().EndBlockingProcess();
    }

    public virtual void SaveGame()
    {

        //sync with cloud
        FirebaseHandler.TryGetInstance().UploadCloudData(ExternalData);

        //save the cache into a local safefile
        MMSaveLoadManager.Save(LocalCache, _cacheFileName, _cacheFolderName);

        Debug.Log("game saved");
    }

    public virtual void CreateDefaultSaveAsync()
    {
        //Initialize a save data with default stats and attributes
        PlayerExternalData default_data = new()
        {
            // the player ID and the display name
            PlayerID = FirebaseHandler.TryGetInstance().TryGetPlayerID(),
            DisplayName = "Default Danny",

            //currency
            Coins = 0,
            Gems = 0,

            //stats of the player character in Edushooter
            EdushooterStats = new()
            {
                Health = 0,
                Damage = 0,
                Speed = 0,
                Dash = 0,
                Reload = 0,

                UnlockedWeapons = new(),
            },

            BestSessions = new List<EdushooterResults>(),

            PlayerUnlockables = new()
            {
                unlockedHeads = new List<ModelType>(),
                unlockedBodies = new List<ModelType>(),
            },

            LastLoginTime = Timestamp.GetCurrentTimestamp(),
            LoginStreak = 0,
        };

        //unlock the default equipment set (AR and GUERILLA skins)
        default_data.EdushooterStats.UnlockedWeapons.Add(WeaponType.AssaultRifle);
        default_data.PlayerUnlockables.unlockedHeads.Add(ModelType.Guerilla);
        default_data.PlayerUnlockables.unlockedBodies.Add(ModelType.Guerilla);

        Debug.Log("creating a new save data and uploading it...");

        // upload the new default save
        FirebaseHandler.TryGetInstance().UploadCloudData(default_data);

        // apply it to the local data
        ExternalData = default_data;

        //turn on renaming panel
        MMGameEvent.Trigger("SummonNamePopup");
    }

    public virtual void CreateNewCache()
    {
        //Initialize an empty cache
        LocalEdushooterStorage default_cache = new()
        {
            EquippedWeapon = WeaponType.AssaultRifle,

            ShuffledMapPlayed = false,
            SelectedMap = MapType.Neighborhood,
            AvailableMapA = MapType.Neighborhood,
            AvailableMapB = MapType.School,

            BodyModel = ModelType.Guerilla,
            HeadModel = ModelType.Guerilla,

            DifficultyLevel = 0,
        };

        // save the newly created cache
        MMSaveLoadManager.Save(default_cache, _cacheFileName, _cacheFolderName);

        LocalCache = default_cache;

        Debug.Log("default cache initialized and saved");
    }

    public virtual void ResetSaveData()
    {
        if (ExternalData != null)
        {
            ExternalData = null;
        }
        Debug.Log("save data reset");
    }

    public virtual void ResetCache()
    {
        //Reset the local cache
        if (LocalCache != null)
        {
            LocalCache = null; 
        }

        MMSaveLoadManager.DeleteSave(_cacheFileName, _cacheFolderName);
        Debug.Log("cache data reset");
    }

    #endregion

    #region Currency Methods
    public void AddCoins(int amount)
    {
        //add the coins but cap it at _CoinCap
        ExternalData.Coins += amount;
        ExternalData.Coins = Mathf.Clamp(ExternalData.Coins, 0, COIN_CAP);

        MMGameEvent.Trigger("CurrencyChanged");
    }

    public bool TryReduceCoins(int amount)
    {
        //if remaining coins after reduction doesnt enter the negatives, reduce it
        if ((ExternalData.Coins - amount) >= 0)
        {
            ExternalData.Coins -= amount;
            MMGameEvent.Trigger("CurrencyChanged");
            return true;
        }
        else return false;
    }

    public void AddGems(int amount)
    {
        //add gems but cap it at _GemCap
        ExternalData.Gems += amount;
        ExternalData.Gems = Mathf.Clamp(ExternalData.Gems, 0, GEM_CAP);

        //upload the changed amount to the firestore manager
        FirebaseHandler.TryGetInstance().UpdateUserGemCount(amount, amount >= 0);

        MMGameEvent.Trigger("CurrencyChanged");
    }

    public bool TryReduceGems(int amount)
    {
        //if remaining gems after reduction doesnt enter the negatives, reduce it
        if ((ExternalData.Gems - amount) >= 0)
        {
            ExternalData.Gems -= amount;

            //upload the changed amount to the firestore manager
            FirebaseHandler.TryGetInstance().UpdateUserGemCount(amount, amount < 0);

            MMGameEvent.Trigger("CurrencyChanged");
            return true;
        }
        else return false;
    }

    public void SetGemAmountNoSync(int amount)
    {
        //this directly sets the gem amount without syncing it with the cloud
        //only use this for initialization!

        Debug.Log("initializing gem count...");
        ExternalData.Gems = Mathf.Clamp(amount, 0, GEM_CAP);
    }

    public int GetGemReward()
    {
        return CalculateGemReward(LocalCache.DifficultyLevel);
    }

#endregion

    #region Encapsulate Stats
    public void UpgradeHealth()
    {
        ExternalData.EdushooterStats.Health++;
    }

    public void UpgradeDamage()
    {
        ExternalData.EdushooterStats.Damage++;
    }

    public void UpgradeSpeed()
    {
        ExternalData.EdushooterStats.Speed++;
    }

    public void UpgradeDash()
    {
        ExternalData.EdushooterStats.Dash++;
    }

    public void UpgradeReload()
    {
        ExternalData.EdushooterStats.Reload++;
    }

    public void ResetAllStats()
    {
        ExternalData.EdushooterStats.Health = 0;
        ExternalData.EdushooterStats.Damage = 0;
        ExternalData.EdushooterStats.Speed = 0;
        ExternalData.EdushooterStats.Dash = 0;
        ExternalData.EdushooterStats.Reload = 0;
    }
#endregion

    #region Edushooter Data Methods

    public bool WeaponIsUnlocked(WeaponType weapon)
    {
        Debug.Log("unlocked weapons: " + ExternalData.EdushooterStats.UnlockedWeapons);
        return ExternalData.EdushooterStats.UnlockedWeapons.Contains(weapon);
    }

    public void UnlockNewWeapon(WeaponType weapon)
    {
        //if we havent unlocked it then add it to the unlocked weapon list
        if (!WeaponIsUnlocked(weapon))
        {
            ExternalData.EdushooterStats.UnlockedWeapons.Add(weapon);
        }
    }

    public bool SetEquippedWeapon(WeaponType weapon)
    {
        //if we're attempting to set a new equipped weapon that hasn't been unlocked yet then reject it
        if (WeaponIsUnlocked(weapon))
        {
            LocalCache.EquippedWeapon = weapon;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UnlockEquippedWeapon(WeaponType weapon, int price)
    {
        //if the weapon is already unlocked then do nothing
        if (WeaponIsUnlocked(weapon))
        {
            return false;
        }

        //if the player can afford it
        if (TryReduceCoins(price))
        {
            //if the weapon isn't inside the list of unlocked weapons then add it
            UnlockNewWeapon(weapon);

            //store the unlocked weapon's data
            FirebaseHandler.TryGetInstance().UploadWeaponPurchase(price, weapon);
            return true;
        }
        else return false;
    }

    public bool GetShuffledMapPlayed()
    {
        return LocalCache.ShuffledMapPlayed;
    }

    public void SetShuffledMapPlayed(bool state)
    {
        LocalCache.ShuffledMapPlayed = state;
    }

    #endregion

    #region PlayerSkinData Methods

    public bool HeadIsUnlocked(ModelType model)
    {
        return ExternalData.PlayerUnlockables.unlockedHeads.Contains(model);
    }

    public bool BodyIsUnlocked(ModelType model)
    {
        return ExternalData.PlayerUnlockables.unlockedBodies.Contains(model);
    }

    public void SetEquippedHead(ModelType model)
    {
        LocalCache.HeadModel = model;
    }

    public void SetEquippedBody(ModelType model)
    {
        LocalCache.BodyModel = model;
    }

    public void UnlockSkin(ModelType model, SkinSelector.SkinType type)
    {
        if (type.Equals(SkinSelector.SkinType.Head))
        {
            ExternalData.PlayerUnlockables.unlockedHeads.Add(model);
        }   
        else
        {
            ExternalData.PlayerUnlockables.unlockedBodies.Add(model);
        }
    }

    #endregion

    #region Firebase Methods

    public string GetPlayerID()
    {
        return ExternalData.PlayerID;
    }

    public void SetPlayerID(string id)
    {
        ExternalData.PlayerID = id;
    }

    #endregion

    #region Cloud Data Methods

    public virtual PlayerExternalData LoadCloudData(FirebaseHandler.EdushooterFirestoreData data)
    {
        if (data == null)
        {
            Debug.Log("no data in cloud detected, making a new template...");
            return null;
        }

        //Initialize a save data with default stats and attributes
        PlayerExternalData migratedData = new()
        {
            // the player ID and the display name
            PlayerID = FirebaseHandler.TryGetInstance().TryGetPlayerID(),
            DisplayName = FirebaseHandler.TryGetInstance().GetPlayerDisplayName(),

            // currencies
            Coins = data.coinCount,
            Gems = FirebaseHandler.TryGetInstance().UserData.gemCount,

            //stats of the player character in Edushooter
            EdushooterStats = new()
            {
                Health = data.abilityData.HealthLevel,
                Damage = data.abilityData.DamageLevel,
                Speed = data.abilityData.SpeedLevel,
                Dash = data.abilityData.DashLevel,
                Reload = data.abilityData.ReloadLevel,

                UnlockedWeapons = data.equipmentData.UnlockedWeapons,
            },

            BestSessions = data.bestSessions,

            PlayerUnlockables = new()
            {
                unlockedHeads = data.equipmentData.UnlockedHeadSkins,
                unlockedBodies = data.equipmentData.UnlockedBodySkins,
            },

            LastLoginTime = data.lastLogin,
            LoginStreak = data.loginStreak,
            HasLoggedInToday = data.hasLoggedIn,
        };

        Debug.Log("loaded data from firestore");
        return migratedData;
    }

    #endregion

    public void AdjustDifficulty(short value)
    {
        LocalCache.DifficultyLevel = value;
    }

    public void SetSelectedMap(MapType selection)
    {
        LocalCache.SelectedMap = selection;
    }

    public string GetDisplayName()
    {
        return ExternalData.DisplayName;
    }

    public void SetBestResults(int diff, MapType map, Timestamp time, int timeToFinish)
    {
        Debug.Log("setting best result");

        //check the list of best sessions
        foreach (EdushooterResults result in ExternalData.BestSessions)
        {
            //if there's already a map inside
            if (result.Map == map)
            {
                //if the results are better then assign it
                //as in, difficulty level is higher than before
                //or the time to finish is higher than before
                if (diff > result.DifficultyLevel || (diff == result.DifficultyLevel && timeToFinish < result.TimeToFinish))
                {
                    result.DifficultyLevel = diff;
                    result.Map = map;
                    result.TimeToFinish = timeToFinish;
                    result.LastFinishTime = time;
                }
                return;
            }
        }

        //when we reach here then there's no map record yet
        //add a new map record

        EdushooterResults newResult = new()
        {
            DifficultyLevel = diff,
            Map = map,
            TimeToFinish = timeToFinish,
            LastFinishTime = time,
        };

        ExternalData.BestSessions.Add(newResult);
        MMGameEvent.Trigger("TriggerSave");
    }

    public EdushooterResults GetBestResult()
    {
        EdushooterResults bestResult = null;

        foreach (EdushooterResults result in ExternalData.BestSessions )
        {
            //assign if null
            if (bestResult == null)
            {
                bestResult = result;
                continue;
            }
            else
            {
                //else, lets compare
                //prioritize results with higher difficulty
                //or faster clear time if its equivalent
                if (bestResult.DifficultyLevel < result.DifficultyLevel 
                    || (bestResult.DifficultyLevel == result.DifficultyLevel && bestResult.TimeToFinish > result.TimeToFinish))
                {
                    bestResult = result;
                }
            }
        }

        return bestResult;
    }

    public EdushooterResults GetMapResult(MapType type)
    {
        return ExternalData.BestSessions.Find(e => e.Map == type);
    }

    public static ProgressManager TryGetInstance()
    {
        return Current;
    }

    public void UploadPlayerData()
    {
        if (ExternalData != null)
        {
            string jsonString = JsonUtility.ToJson(ExternalData);
            Debug.Log(jsonString);

            Dictionary<string, object> jsonDictionary = JsonUtility.FromJson<Dictionary<string, object>>(jsonString);
        }
        else
        {
            Debug.LogError("Trying to upload an empty player data!");
        }
    }

    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        switch (gameEvent.EventName)
        {
            case "TriggerSave":
                SaveGame();
                break;
            case "DebugAddCoins":
                TestAddCoins();
                break;
        }
    }
    #endregion
}
