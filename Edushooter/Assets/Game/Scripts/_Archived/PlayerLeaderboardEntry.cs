using MoreMountains.Tools;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLeaderboardEntry : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI MapText;
    public TextMeshProUGUI DifficultyText;

    //cache
    [Space]
    public string initialDateText, initialMapText, initialDifficultyText;

    private void Start()
    {
        _Initialize();
    }

    private void ResetEntry()
    {
        DateText.text = initialDateText;
        MapText.text = initialMapText;
        DifficultyText.text = initialDifficultyText;
    }

    public void _Initialize()
    {
        SetFields(ProgressManager.TryGetInstance().GetBestResult());
    }

    public void SetEntryValue(MapType map)
    {
        Debug.Log("found leaderboard data for map " + map.ToString());
        SetFields(ProgressManager.TryGetInstance().GetMapResult(map));
    }

    private void SetFields(EdushooterResults setResult)
    {

        if (setResult != null)
        {
            DateText.text = MiscellaneousMethods.ConvertTimestampToDateString(setResult.LastFinishTime);
            MapText.text = setResult.Map.ToString();
            DifficultyText.text = setResult.DifficultyLevel.ToString();
        }
        else
        {
            ResetEntry();
        }
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
