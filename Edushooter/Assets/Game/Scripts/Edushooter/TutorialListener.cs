using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialListener : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public LevelSelector LevelSelector;
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
        if (eventType.EventName == "GoToTutorial")
        {
            LevelSelector.GoToLevel();
        }
    }
}
