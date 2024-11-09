using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPauseManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [SerializeField]
    [MMReadOnly]
    private bool _paused = false;

    private void Pause()
    {
        //if the game is not paused then pause it
        if (!_paused)
        {
            //trigger the pause
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
            _paused = true;

            //turn on the pause screen
            GUIManager.Instance.SetPauseScreen(true);

            MMGameEvent.Trigger("InitializeVolumeSettings");
        }
    }

    private void Unpause()
    {
        if (_paused)
        {
            //resume time
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 1f, 0f, false, 0f, true);
            _paused = false; 

            //turn off the pause screen
            GUIManager.Instance.SetPauseScreen(false);
        }
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
            case "PauseSession":
                Pause();
                break;

            // the signal that countdown has ended and we're ready to start spawning
            case "UnpauseSession":
                Unpause();
                break;
        }
    }
    #endregion
}
