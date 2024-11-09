using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class AnnouncerManager : MMSingleton<AnnouncerManager>, MMEventListener<MMGameEvent>
{
    public MMF_Player[] gameStartClips;
    public MMF_Player[] gameEndClips; 
    public MMF_Player[] intermissionClips;
    public MMF_Player[] playerDeathClips; 

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
            case "StartSessionGameplay":
                PlayRandomClip(gameStartClips);
                break;
            case "ToggleEndText":
                PlayRandomClip(gameEndClips);
                break;
            case "IntermissionStart":
                PlayRandomClip(intermissionClips);
                break;
            case "PlayerDied":
                PlayRandomClip(playerDeathClips);
                break;
        }
    }
    #endregion

    private void PlayRandomClip(MMF_Player[] clips)
    {
        if (clips.Length == 0) return;
        int randomIndex = Random.Range(0, clips.Length);
        clips[randomIndex].PlayFeedbacks();
    }
}
