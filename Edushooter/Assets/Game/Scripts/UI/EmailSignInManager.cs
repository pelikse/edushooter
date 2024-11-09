using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class EmailSignInManager : MonoBehaviour
{
    [Header("Email Account Panels")]
    [SerializeField] private GameObject SigninPanel;

    [Space, Space]
    [Header("Sign Up Fields")]
    [SerializeField] private TMP_InputField SignUpEmail;
    [SerializeField] private TMP_InputField SignUpPassword;

    [Space]

    [SerializeField] private PopupInfo NotifySignedIn;

    [Space]
    
    [Header("Warnings")]
    [SerializeField] private PopupInfo WarningFalseCredentials;
    [SerializeField] private PopupInfo WarningHasWhitespace;

    // cache
    private string _email, _password;

    private void Start()
    {
        SigninPanel.SetActive(false);
    }

    public void PromptSignIn()
    {
        SigninPanel.SetActive(true);
    }

    public void CloseSignIn()
    {
        //reset fields
        SignUpEmail.text = "";
        SignUpPassword.text = "";

        //close the panel
        SigninPanel.SetActive(false);
    }

    public async void AttemptSignIn()
    {
        // fetch email and password data
        _email = SignUpEmail.text;
        _password = SignUpPassword.text;

        // if the password or email is wrong, then stop
        if (HasWhitespace(_email) || HasWhitespace(_password))
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningHasWhitespace);
            return;
        }

        LoadingOverlayManager.TryGetInstance()?.AddBlockingProcess();

        // attempt to sign in the user
        Debug.Log("checking firebase for user");
        bool _isSuccess = await FirebaseHandler.TryGetInstance().SignInUser(_email, _password);
        if (_isSuccess)
        {
            CloseSignIn();

            LandingManager.TryGetInstance().SuccessfulSignupSwitchToSignedPanel();
        }
        else
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningFalseCredentials);
        }

        LoadingOverlayManager.TryGetInstance()?.EndBlockingProcess();
    }

    private bool HasWhitespace(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
