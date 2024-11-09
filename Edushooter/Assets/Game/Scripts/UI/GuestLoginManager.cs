using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestLoginManager : MonoBehaviour
{
    [SerializeField] private GameObject GuestLoginPanel;

    // Start is called before the first frame update
    void Start()
    {
        GuestLoginPanel.SetActive(false);
    }

    public void PromptGuestLogin()
    {
        GuestLoginPanel.SetActive(true);
    }

    public void CloseGuestLogin()
    {
        GuestLoginPanel.SetActive(false);
    }

    public async void AttemptGuestLogin()
    {
        LoadingOverlayManager.TryGetInstance()?.AddBlockingProcess();

        bool _success = await FirebaseHandler.TryGetInstance().SignInAnonymously();

        if (_success)
        {
            CloseGuestLogin();
            LandingManager.TryGetInstance().SuccessfulSignupSwitchToSignedPanel();
        }

        LoadingOverlayManager.TryGetInstance().EndBlockingProcess();
    }
}
