using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSessionTextListener : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
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
        if (eventType.EventName == "ToggleEndText")
        {
            text.gameObject.SetActive(true);
        }
    }
    #endregion
}
