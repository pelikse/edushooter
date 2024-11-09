using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
public class AIActionInvokeFunction : AIAction
{
    //a list of MMF Player to play
    public MMF_Player Feedback;
    //should the feedback play once EVERY TIME IT ENTERS THIS STATE
    public bool PlayOnce = true;
    public UnityEvent Function;

    private bool _enteredState = false;
    public override void PerformAction()
    {
        if (_enteredState && PlayOnce) return;

        _enteredState = true;
        Function?.Invoke();
    }

    public override void OnExitState()
    {
        _enteredState = false;
    }
}
