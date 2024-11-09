using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomMapSelector : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [Header("Map Database")]
    public MapDatabase MapList;

    [Space]

    public MapChoice MapA;
    public MapChoice MapB;

    private void _Initialize()
    {
        //if they have played the shuffled map then
        if (ProgressManager.TryGetInstance().GetShuffledMapPlayed())
        {
            MapInfo[] _mapIndex = MapList.GetTwoDistinctMapTypes();

            ProgressManager.TryGetInstance().LocalCache.AvailableMapA = _mapIndex[0].mapType;
            ProgressManager.TryGetInstance().LocalCache.AvailableMapB = _mapIndex[1].mapType;

            //set map A as the default selected map
            ProgressManager.TryGetInstance().SetSelectedMap(_mapIndex[0].mapType);

            ProgressManager.TryGetInstance().SetShuffledMapPlayed(false);
        }

        MapA.SetMap(MapList.GetMapFromType(ProgressManager.TryGetInstance().LocalCache.AvailableMapA));
        MapB.SetMap(MapList.GetMapFromType(ProgressManager.TryGetInstance().LocalCache.AvailableMapB));

        //fetch the selected map data
        MapA.FetchMapData();
        MapB.FetchMapData();
    }

    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName.Equals("SwitchedPanel"))
        {
            _Initialize();
        }
    }
    #endregion
}
