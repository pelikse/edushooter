using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapChoice : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public Image MapImage;
    public TextMeshProUGUI MapTitle;
    public ModifiableButtons BackgroundSprite;

    [Space]

    public TextMeshProUGUI MapDescription;

    [Space]

    public Sprite SelectedSprite;
    public Sprite UnselectedSprite;

    [SerializeField] private MapInfo CurrentMap;


    public void SetMap(MapInfo map)
    {
        CurrentMap = map;
    }

    public void FetchMapData()
    {
        MapImage.sprite = CurrentMap.mapSprite;
        MapTitle.text = CurrentMap.mapName;

        SetMapSelectionStatus();
    }

    public void SetMapSelectionStatus()
    {
        //if this map is the selected map
        if (CurrentMap.mapType == ProgressManager.TryGetInstance().LocalCache.SelectedMap)
        {
            //change the color to reflect map selection
            MapTitle.color = new Color(0.88f, 1f, 0.807f);
            MapDescription.text = CurrentMap.mapDesc;

            //change background sprite
            BackgroundSprite.ChangeInitialSprite(SelectedSprite);
        }
        else
        {
            MapTitle.color = Color.white;
            BackgroundSprite.ChangeInitialSprite(UnselectedSprite);
        }
    }

    public void SelectMap()
    {
        ProgressManager.TryGetInstance().SetSelectedMap(CurrentMap.mapType);
        MMGameEvent.Trigger("SwitchedMap");
        MMGameEvent.Trigger("TriggerSave");
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
        if (gameEvent.EventName.Equals("SwitchedMap"))
        {
            SetMapSelectionStatus();
        }
    }
    #endregion
}
