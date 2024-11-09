using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorIndicatorManager : MMSingleton<ArmorIndicatorManager>, MMEventListener<MMGameEvent>
{
    public GameObject ArmorEffect;
    [Space]
    public MMF_Player ArmorChargeFeedback;
    public MMF_Player ArmorDamageFeedback;
    public MMF_Player ArmorBreakFeedback;

    private void Start()
    {
        ArmorEffect.SetActive(false);
    }

    #region EventListener Implement
    private void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        switch (eventType.EventName)
        {
            case "ArmorDamage":
                ArmorDamageFeedback.PlayFeedbacks();
                break;

            // the signal that countdown has ended and we're ready to start spawning
            case "ArmorBreak":
                ArmorEffect.SetActive(false);
                ArmorBreakFeedback.PlayFeedbacks();
                break;

            // listen to the signal of the end of preparation, start countdown
            case "ArmorCharge":
                ArmorEffect.SetActive(true);
                ArmorChargeFeedback.PlayFeedbacks();
                break;
        }
    }
    #endregion
}
