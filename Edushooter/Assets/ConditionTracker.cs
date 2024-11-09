using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionTracker : MonoBehaviour
{
    public MMRadialProgressBar Gauge;
    public Image BGColor;
    public Image Icon;

    private bool _isActive;
    private float _currentDuration, _maxDuration;

    public void InitializeCondition(float maxDuration, Color bgColor, Sprite icon)
    {
        Debug.Log("initializing new tracker with duration " + maxDuration + " color " + bgColor);

        _currentDuration = maxDuration;
        _maxDuration = maxDuration;

        BGColor.color = bgColor;

        if (icon != null )
        {
            Icon.sprite = icon;
        }

        _isActive = true;
    }

    private void Update()
    {
        if (_isActive)
        {
            _currentDuration -= Time.deltaTime;

            if (_currentDuration <= 0f )
            {
                _isActive = false;
                //return to pool
                gameObject.SetActive(false);
            }
            else
            {
                Gauge.UpdateBar(_currentDuration, _maxDuration, 0f);
            }
        }
    }
}
