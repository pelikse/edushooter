using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MMSingleton<SFXPlayer>
{
    [Space,Space]

    [SerializeField] private MMF_Player NormalButtonPress;
    [SerializeField] private MMF_Player EmphasisButtonPress;
    [SerializeField] private MMF_Player CancelButtonPress;
    [SerializeField] private MMF_Player PurchaseSFX;
    [SerializeField] private MMF_Player UpgradeSFX;
    [SerializeField] private MMF_Player PopupSFX;

    [Space]
    [SerializeField] private AudioSource Player;

    #region SFX Methods
    public void PlayNormalButtonPress() {  
        NormalButtonPress.PlayFeedbacks();
    }

    public void PlayEmphasisButtonPress() {
        EmphasisButtonPress.PlayFeedbacks();
    }
    public void PlayCancelButtonPress(){
        CancelButtonPress.PlayFeedbacks();
    }

    public void PlayPurchaseSFX()
    {
        PurchaseSFX.PlayFeedbacks();
    }

    public void PlayUpgradeSFX() 
    {
        UpgradeSFX.PlayFeedbacks();
    }

    public void PlayPopupSFX()
    {
        PopupSFX.PlayFeedbacks();
    }
    #endregion
}
