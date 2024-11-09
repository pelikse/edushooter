using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapSelector : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public TextMeshProUGUI SelectedText;

    [TextArea(3,3)]
    public string SelectionFormat = "Selected Map: {0}";

    private void SetSelectedText(string mapName)
    {
        SelectedText.text = string.Format(SelectionFormat, mapName);
    }

    public void SelectNeighborhood()
    {
        ProgressManager.TryGetInstance().SetSelectedMap(MapType.Neighborhood);
        SetSelectedText(MapType.Neighborhood.ToString());

        MMGameEvent.Trigger("TriggerSave");
    }

    public void SelectMeadows()
    {
        ProgressManager.TryGetInstance().SetSelectedMap(MapType.Meadows);
        SetSelectedText(MapType.Meadows.ToString());

        MMGameEvent.Trigger("TriggerSave");
    }

    public void SelectWarehouse()
    {
        ProgressManager.TryGetInstance().SetSelectedMap(MapType.Warehouse);
        SetSelectedText(MapType.Warehouse.ToString());

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
        switch (gameEvent.EventName)
        {
            case "SwitchedPanel":
                SetSelectedText(ProgressManager.TryGetInstance().LocalCache.SelectedMap.ToString());
                break;
        }
    }
    #endregion
}
