using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFetcher : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [SerializeField] public DataFetcher[] Fetchers;

    public void InitializePanel()
    {
        PlayerExternalData data = ProgressManager.TryGetInstance().ExternalData;
        Fetchers[0].FetchData(data);
        
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
        throw new System.NotImplementedException();
    }

}
