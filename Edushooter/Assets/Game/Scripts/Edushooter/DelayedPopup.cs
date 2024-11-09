using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedPopup : MonoBehaviour
{
    [MMInformation("A script that either invokes a function or summons a popup when bound to a button. If the user holds the button down longer than PopupTimeLimit, then summon a Popup. If not, then perform an action.", MMInformationAttribute.InformationType.None, false)]
    public float PopupTimeLimit = 1f;
    public PopupInfo PopupInfo;
    public UnityEvent OnShortPress;

    private bool _pressed = false;
    private float _timeStamp;

    public void ButtonPressed()
    {
        Debug.Log("Button is pressed");
        _pressed = true;
        _timeStamp = Time.time;
    }

    public void ButtonReleased()
    {
        
        if (!_pressed)
        {
            Debug.LogError("Navigation Button is released before it is even pressed on " + gameObject);
            return;
        }

        _pressed = false;

        //if the button is held longer than PopupTimeLimit then display a popup
        if (Time.time - _timeStamp > PopupTimeLimit)
        {
            if (PopupInfo != null && PopupElement.TryGetInstance() != null)
            {
                PopupElement.Current.DisplayPopup(PopupInfo);
            }
        }
        //if not then navigate scenes
        else
        {
            OnShortPress?.Invoke();
        }
    }
}
