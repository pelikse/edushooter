using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialPopupManager : MMSingleton<TutorialPopupManager>
{
    public GameObject PopupContainer;

    public VideoPlayer VideoPlayer;
    public TextMeshProUGUI TutorialDesc;
    public TextMeshProUGUI TutorialName;
    

    public void TriggerTutorialPopup(TutorialPopup popup)
    {
        //pause the game
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);

        SFXPlayer.TryGetInstance().PlayPopupSFX();

        PopupContainer.SetActive(true);

        TutorialDesc.text = popup.PopupDesc;
        TutorialName.text = popup.PopupTitle;

        VideoPlayer.clip = popup.PopupVideoClip;
        VideoPlayer.Play();
    }

    public void CloseTutorialPopup()
    {
        //turn off the popup
        PopupContainer.SetActive(false);

        //resume time
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 1f, 0f, false, 0f, true);
    }

    private void Start()
    {
        //turn off the popup
        if (PopupContainer.activeInHierarchy)
        {
            PopupContainer.SetActive(false);
        }
    }
}
