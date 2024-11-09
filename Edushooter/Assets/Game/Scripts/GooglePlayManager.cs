
using UnityEngine;

//using GooglePlayGames;
//using GooglePlayGames.BasicApi;

using Firebase.Auth;
using MoreMountains.Tools;


public class GooglePlayManager : MonoBehaviour
{
    //[SerializeField] private bool SignInOnStart = false;

    ////cache
    //private Credential credential;
    //private string Token;

    //private void Start()
    //{
    //    if (SignInOnStart)
    //    {
    //        AutoSignIn();
    //    }
    //}

    //public void AutoSignIn()
    //{
    //    Debug.Log("attempting auto sign in...");
    //    MMGameEvent.Trigger("ActivateLoading");
    //    PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    //}

    //public void SignIn()
    //{
    //    Debug.Log("attempting manual sign in...");
    //    MMGameEvent.Trigger("ActivateLoading");
    //    PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    //}

    //private async void AttemptFirebaseAuth()
    //{
    //    bool authStatus = await FirebaseHandler.TryGetInstance().SignInWithGooglePlay(credential);

    //    if (authStatus)
    //    {
    //        LandingManager.TryGetInstance().SwitchPanelSignedIn();
    //    }

    //    MMGameEvent.Trigger("DeactivateLoading");
    //}

    //internal void ProcessAuthentication(SignInStatus status)
    //{
    //    if (status == SignInStatus.Success)
    //    {
    //        // Continue with Play Games Services
    //        Debug.Log("sign in to google play successful!");

    //        // authenticate with firebase
    //        PlayGamesPlatform.Instance.RequestServerSideAccess(false, authCode =>
    //        {
    //            Debug.Log("authcode is " + authCode);   
    //            Token = authCode;

    //            if (Token != null)
    //            {
    //                credential = PlayGamesAuthProvider.GetCredential(Token);

    //                AttemptFirebaseAuth();
    //            }
    //        });
    //    }
    //    else
    //    {
    //        // Disable your integration with Play Games Services or show a login button
    //        // to ask users to sign-in. Clicking it should call
    //        // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
    //        MMGameEvent.Trigger("DeactivateLoading");

    //        Debug.Log("FAILURE to sign in with google play!");
    //        PopupElement.Current.DisplayPopup("Google Play Sign In Failure!", "You failed to sign in with <color=#0F9D58>Google Play Services</color>, please try again!");
    //    }

    //    Debug.Log(status.ToString());
    //}
}
