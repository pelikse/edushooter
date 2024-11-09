using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class AIActionPlayFeedback : AIAction
{
    //a list of MMF Player to play
    public MMF_Player Feedback;
    //should the feedback play once EVERY TIME IT ENTERS THIS STATE
    public bool PlayOnce = true;

    private bool _enteredState = false;
    public override void PerformAction()
    {
        if (_enteredState && PlayOnce) return;
        
        _enteredState = true;
        Feedback.PlayFeedbacks();
    }

    public override void OnExitState()
    {
        _enteredState = false;
    }
}
