using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyInfoFetcher : MonoBehaviour, DataFetcher, MMEventListener<MMGameEvent>
{
    [SerializeField] private TextMeshProUGUI CoinsText;
    [SerializeField] private TextMeshProUGUI GemsText;

    public void FetchData(PlayerExternalData data)
    {
        CoinsText.text = data.Coins.ToString();
        GemsText.text = data.Gems.ToString();
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
        if (eventType.EventName.Equals("CurrencyChanged") || eventType.EventName.Equals("SwitchedPanel"))
        {
            FetchData(ProgressManager.TryGetInstance().ExternalData);
        }
    }
}
