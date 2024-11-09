using Firebase.Firestore;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionFinishedCaller : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public StopwatchScript timer;
    public SessionManager sessionManager;


    #region EventListener Implement
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        //if the player wins
        if (eventType.EventName.Equals("SessionFinished"))
        {
            FirebaseHandler.TryGetInstance().UploadEdushooterResults(true, timer.GetCurrentTime().Seconds, ProgressManager.TryGetInstance().LocalCache.DifficultyLevel, sessionManager.CoinReward, sessionManager.GetEnemiesKilled(), ProgressManager.TryGetInstance().LocalCache.SelectedMap);

            //upload data to personal leaderboard too
            ProgressManager.TryGetInstance().SetBestResults(ProgressManager.TryGetInstance().LocalCache.DifficultyLevel, ProgressManager.TryGetInstance().LocalCache.SelectedMap, Timestamp.FromDateTime(DateTime.UtcNow), timer.GetCurrentTime().Seconds);
            MMGameEvent.Trigger("TriggerSave");        }

        //if the player loses
        else if (eventType.EventName.Equals("PlayerDied"))
        {
            FirebaseHandler.TryGetInstance().UploadEdushooterResults(false, timer.GetCurrentTime().Seconds, ProgressManager.TryGetInstance().LocalCache.DifficultyLevel, sessionManager.CoinReward, sessionManager.GetEnemiesKilled(), ProgressManager.TryGetInstance().LocalCache.SelectedMap);
            //set shuffled map so we can get new maps
            ProgressManager.TryGetInstance().SetShuffledMapPlayed(true);
            //play player died animation
            MMGameEvent.Trigger("CharacterAnimationTriggerFailure");
        }
    }
    #endregion
}
