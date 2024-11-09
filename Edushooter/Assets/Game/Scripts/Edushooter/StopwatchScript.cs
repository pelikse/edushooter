using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StopwatchScript : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public TextMeshProUGUI TimeText;

    [SerializeField]
    [MMReadOnly]
    private bool _timerActive = false;

    [SerializeField]
    [MMReadOnly]
    private float _currentTime = 0f;

    private TimeSpan _time;

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
        if (eventType.EventName.Equals("StartSessionGameplay"))
        {
            _timerActive = true;
        }
        else if (eventType.EventName.Equals("SessionFinished") || eventType.EventName.Equals("PlayerDied"))
        {
            _timerActive = false;
        }
    }

    private void Update()
    {
        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
            _time = TimeSpan.FromSeconds(_currentTime);

            TimeText.text = string.Format("{0}:{1}.<size=-20>{2}</size>", _time.Minutes.ToString("D2"), _time.Seconds.ToString("D2"), _time.Milliseconds.ToString("D3"));
        }
    }

    public TimeSpan GetCurrentTime()
    {
        return _time;
    }
}
