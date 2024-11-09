using Firebase.Firestore;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardStatusManager : MonoBehaviour
{
    //public RewardStatus SuccessReward;
    //public RewardStatus PerfectReward;

    [Space]

    public PopupInfo RefreshPopup;

    //private int successRewardState;
    //private int perfectRewardState;

    // Start is called before the first frame update
    void Start()
    {
        //DateTime _lastReward = ProgressManager.TryGetInstance().PlayerData.EdugymStats.lastRewardDate;

        //Debug.Log("last time play is at " + _lastReward);

        ////check if the date has changed and refresh rewards if so
        //if (DateTime.Today > _lastReward)
        //{
        //    Debug.Log("refreshed rewards because date has changed to " + DateTime.Today);
        //    ProgressManager.TryGetInstance().SetEdugymPenalty(PlayerEdugymData.EdugymState.Perfect, 0);
        //    ProgressManager.TryGetInstance().SetEdugymPenalty(PlayerEdugymData.EdugymState.Success, 0);

        //    //save the state
        //    MMGameEvent.Trigger("TriggerSave");

        //    StartCoroutine(PopupDelay());
        //}
        //else
        //{
        //    Debug.Log("date hasn't changed");
        //}

        //reward notification disabled since its diminishing returns
        /*
        //get the reward availability
        successRewardState = ProgressManager.TryGetInstance().GetEdugymPenalty(PlayerEdugymData.EdugymState.Success);
        perfectRewardState = ProgressManager.TryGetInstance().GetEdugymPenalty(PlayerEdugymData.EdugymState.Perfect);

        //set the state
        SuccessReward.FetchData(successRewardState > 0);
        PerfectReward.FetchData(perfectRewardState > 0);
        */
    }

    private IEnumerator PopupDelay()
    {
        yield return new WaitForSeconds(1f);

        PopupElement.TryGetInstance().DisplayPopup(RefreshPopup);
    }
}
