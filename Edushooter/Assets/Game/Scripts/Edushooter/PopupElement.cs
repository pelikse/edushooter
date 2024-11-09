using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupElement : MMSingleton<PopupElement>
{
    [SerializeField] private CanvasGroup PopupGroup;
    [SerializeField] private MMF_Player PopupSource;
    [Space]
    [SerializeField] private TextMeshProUGUI PopupTitle;
    [SerializeField] private TextMeshProUGUI PopupDescription;

    private void Start()
    {
        //if the popup is active, then turn it off
        if (PopupGroup.gameObject.activeInHierarchy)
        {            
            PopupGroup.alpha = 0f;
            PopupGroup.gameObject.SetActive(false);

        }
    }

    public void DisplayPopup(PopupInfo info)
    {
        PopupGroup.gameObject.SetActive(true);
        PopupGroup.alpha = 0f;

        //load the info for the popup
        PopupTitle.text = info.PopupTitle;
        PopupDescription.text = info.PopupDesc;

        PopupGroup.alpha = 1f;
    }

    public void DisplayPopup(string title, string desc)
    {
        PopupGroup.gameObject.SetActive(true);
        PopupGroup.alpha = 0f;

        //load the info for the popup
        PopupTitle.text = title;
        PopupDescription.text = desc;

        PopupGroup.alpha = 1f;
    }

    public void ClosePopup()
    {
        PopupGroup.alpha = 0f;
        PopupGroup.gameObject.SetActive(false);
    }

    public void PlayPopupSound()
    {
        PopupSource.PlayFeedbacks();
    }
}
