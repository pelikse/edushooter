using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerListener : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [System.Serializable]
    public enum EventTypes
    {
        MMGameEvent,
    }

    public EventTypes EventTypeListened = EventTypes.MMGameEvent;
    public string ListenedName;

    [Space]

    public Animator Animator;
    public string TriggerName;


    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName.Equals(ListenedName))
        {
            Animator.SetTrigger(TriggerName);
        }
    }
    #endregion
}
