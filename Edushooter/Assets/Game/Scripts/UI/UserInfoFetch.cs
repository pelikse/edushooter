using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoFetch : MonoBehaviour, DataFetcher, MMEventListener<MMGameEvent>
{
    [SerializeField] private TextMeshProUGUI CharacterName;

    public void FetchData(PlayerExternalData data)
    {
        CharacterName.text = data.DisplayName.ToString();
    }

    private void Start()
    {
        //fetch all required data and display it
        FetchData(ProgressManager.TryGetInstance().ExternalData);
    }

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
        if (eventType.EventName.Equals("SwitchedPanel"))
        {
            FetchData(ProgressManager.TryGetInstance().ExternalData);
        }
    }

}
