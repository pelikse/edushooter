using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHUD : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public CanvasGroup HUD;

    // Start is called before the first frame update
    void Start()
    {
        HUD.alpha = 0;
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
        if (eventType.EventName == "TurnOffHUD")
        {
            HUD.alpha = 0;
        }
        else if (eventType.EventName == "TurnOnHUD")
        {
            HUD.alpha = 1;
        }
    }
    #endregion
}
