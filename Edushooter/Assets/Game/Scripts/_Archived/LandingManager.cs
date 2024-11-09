using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingManager : MMSingleton<LandingManager>
{
    [System.Serializable]
    public enum InitializedGameState
    {
        Initializing,
        Unsigned,
        SignedIn
    }

    private InitializedGameState landingState = InitializedGameState.Initializing;
    public InitializedGameState LandingState { get => landingState; }

    [Space, Space]

    [SerializeField] private GameObject InitializingPanel;
    [SerializeField] private GameObject UnsignedPanel;
    [SerializeField] private GameObject SignedInPanel;

    [Space]

    [SerializeField] private FirebaseHandler FirebaseHandler;
    [SerializeField] private float MaximumWaitTime = 10f;

    [Space]

    [SerializeField] private QuestionInfo RestartLoadingQuestion;
    [SerializeField] private QuestionInfo SignOutConfirmation;

    private bool _waiting = true;
    private float _startWait;


    public void InitializeLanding()
    {
        // stop waiting
        _waiting = false;
        InitializingPanel.SetActive(false);

        // check for authentication
        if (FirebaseHandler.TryGetInstance().IsSignedIn())
        {
            landingState = InitializedGameState.SignedIn;

            Debug.Log("player is signed in");
            SwitchPanelSignedIn();
        }
        else
        {
            landingState = InitializedGameState.Unsigned;

            Debug.Log("player is signed out");
            UnsignedPanel.SetActive(true);
        }
    }

    public void AttemptSignOut()
    {
        // create a callback
        void signOutCallback()
        {
            FirebaseHandler.TryGetInstance().SignOut(() =>
            {
                landingState = InitializedGameState.Unsigned;
                SignedInPanel?.SetActive(false);
                UnsignedPanel?.SetActive(true);
            });
        }

        // ask for confirmation before actually signing out
        QuestionElement.TryGetInstance().DisplayQuestion(SignOutConfirmation, signOutCallback);
    }
    
    public void SwitchPanelSignedIn()
    {
        UnsignedPanel?.SetActive(false);
        InitializingPanel?.SetActive(false);

        SignedInPanel?.SetActive(true);

        // Load progress
        ProgressManager.TryGetInstance().LoadSave();
    }

    public void SuccessfulSignupSwitchToSignedPanel()
    {
        SwitchPanelSignedIn();
        PopupElement.TryGetInstance().DisplayPopup("Sign Up Successful!", "You have successfully signed in, enjoy Edushooter!");
    }

    // Start is called before the first frame update
    void Start()
    {
        // display the loading screen and turn off other panels
        UnsignedPanel.SetActive(false);
        SignedInPanel.SetActive(false);

        InitializingPanel.SetActive(true);

        // initialize firebase and wait for callback
        FirebaseHandler.InitializeFirebase();

        _startWait = Time.time;
        _waiting = true;
    }

    private void Update()
    {
        if (_waiting)
        {
            // if we've been waiting for firebase for more than our maximum wait time...
            if ((Time.time  - _startWait) > MaximumWaitTime)
            {
                // pause waiting
                _waiting = false;

                // define the callback
                QuestionElement.AcceptCallback callback = () =>
                {
                    Debug.Log("quit application");

                    Application.Quit();
                };

                // notify players and ask if they want to restart
                QuestionElement.TryGetInstance().DisplayQuestion(RestartLoadingQuestion, callback);
            }
        }
    }
}
