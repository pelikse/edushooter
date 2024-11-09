using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using MoreMountains.Tools;

public class EmailSignUpManager : MonoBehaviour
{
    [Header("Email Account Panels")]
    [SerializeField] private GameObject SignupPanel;

    [Space, Space]
    [Header("Sign Up Fields")]
    [SerializeField] private TMP_InputField SignUpEmail;
    [SerializeField] private TMP_InputField SignUpPassword;
    [SerializeField] private TMP_InputField PasswordConfirm;

    [Space]

    [SerializeField] private PopupInfo NotifySuccessfulSignup;

    [Header("Warnings")]
    [SerializeField] private PopupInfo WarningFailedSignup;

    [Space]

    [SerializeField] private PopupInfo WarningPasswordNotConfirmed;
    [SerializeField] private PopupInfo WarningHasNullOrWhitespace;
    [SerializeField] private PopupInfo WarningShortLength;
    [SerializeField] private PopupInfo WarningNoDigits;
    [SerializeField] private PopupInfo WarningNoSpecial;

    // cache
    private string _email, _password, _confirmation;

    const short MINIMUM_PASSWORD_LENGTH = 6;

    // password pattern
    const string digitPattern = @"\d";
    const string specialCharPattern = @"[@$!%*?&_-]";

    private void Start()
    {
        SignupPanel.SetActive(false);
    }

    public void PromptSignup()
    {
        SignupPanel.SetActive(true);
    }

    public void CloseSignup()
    {
        //reset fields
        SignUpEmail.text = "";
        SignUpPassword.text = "";
        PasswordConfirm.text = "";

        //close the panel
        SignupPanel.SetActive(false);
    }

    public async void AttemptSignUp()
    {
        _email = SignUpEmail.text;
        _password = SignUpPassword.text;
        _confirmation = PasswordConfirm.text;

        // if the password is wrong, then stop
        if (!PasswordIsValid())
        {
            return;
        }

        LoadingOverlayManager.TryGetInstance()?.AddBlockingProcess();

        // wait for the state
        bool _signUpSuccessful = await FirebaseHandler.TryGetInstance().SignUpUser(_email, _password);

        if (_signUpSuccessful)
        {
            Debug.Log("successful sign up!");
            CloseSignup();

            // switch on the panel for signed in users
            LandingManager.TryGetInstance().SuccessfulSignupSwitchToSignedPanel();
        }

        LoadingOverlayManager.TryGetInstance()?.EndBlockingProcess();
    }

    public async void AttemptSignUpAndLink()
    {
        _email = SignUpEmail.text;
        _password = SignUpPassword.text;
        _confirmation = PasswordConfirm.text;

        // if the password is wrong, then stop
        if (!PasswordIsValid())
        {
            return;
        }

        LoadingOverlayManager.TryGetInstance()?.AddBlockingProcess();

        // wait for the state
        bool _signUpSuccessful = await FirebaseHandler.TryGetInstance().LinkUserEmailCredentials(_email, _password);

        if (_signUpSuccessful)
        {
            Debug.Log("successful sign up!");
            PopupElement.TryGetInstance()?.DisplayPopup("Successful Link!", "Your account has successfully been linked to the " + _email + " account.");
            CloseSignup();
        }

        LoadingOverlayManager.TryGetInstance()?.EndBlockingProcess();
    }

    private bool PasswordIsValid()
    {
        //if there's any whitspace or null characters then its invalid
        if (string.IsNullOrWhiteSpace(_password))
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningHasNullOrWhitespace);
            return false;
        }

        //if the passwords dont match with the confirmation
        if (!_password.Equals(_confirmation))
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningPasswordNotConfirmed);
            return false;
        }

        // if the length of the password is too short then its invalid
        if (_password.Length < MINIMUM_PASSWORD_LENGTH)
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningShortLength);
            return false;
        }

        // check for expressions
        bool hasDigit = Regex.IsMatch(_password, digitPattern);
        bool hasSpecialChar = Regex.IsMatch(_password, specialCharPattern);

        // if the password has no numbers
        if (!hasDigit)
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningNoDigits);
            return false;
        }

        // if the password has no special characters
        if (!hasSpecialChar)
        {
            PopupElement.TryGetInstance().DisplayPopup(WarningNoSpecial);
            return false;
        }

        Debug.Log("password is valid");
        // everything is ok, password is valid
        return true;
    }
}
