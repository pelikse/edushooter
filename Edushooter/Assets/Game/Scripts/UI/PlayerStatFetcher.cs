using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatFetcher : MonoBehaviour, DataFetcher, MMEventListener<MMGameEvent>
{
    public Slider HitPoints;
    public Slider Damage;
    public Slider MovementSpeed;
    public Slider DashBoost;
    public Slider Reload;

    [SerializeField]
    private CharacterUpgradeManager UpgradeManager;

    public void FetchData(PlayerExternalData data)
    {
        int _levelCap = ProgressManager.TryGetInstance().LevelCap;

        //fetchs data from save file and displays it as a slider
        HitPoints.value = data.EdushooterStats.Health / (float)_levelCap;
        Damage.value = data.EdushooterStats.Damage / (float)_levelCap;
        MovementSpeed.value = data.EdushooterStats.Speed / (float)_levelCap;
        DashBoost.value = data.EdushooterStats.Dash / (float)_levelCap;
        Reload.value = data.EdushooterStats.Reload / (float)_levelCap;
    }

    private void Start()
    {
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
        if (eventType.EventName.Equals("CharacterStatChanged") || eventType.EventName.Equals("SwitchedPanel"))
        {
            FetchData(ProgressManager.TryGetInstance().ExternalData);
        }
    }

}
