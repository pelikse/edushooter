using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGaugeManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public MMRadialProgressBar Gauge;
    [SerializeField]
    [MMReadOnly]
    protected CharacterHandleWeapon WeaponHandler;

    protected int maxAmmo;
    protected float reloadDuration;
    protected float _reloadCounter;

    protected bool reloading = false;
    private bool initialized = false;

    // attempt a reload, this initiates the reload gauge
    public void StartReload()
    {
        //if already reloading, then do nothing
        if (reloading) return;

        //else start reload
        reloadDuration = WeaponHandler.CurrentWeapon.ReloadTime + 0.05f;

        reloading = true;
        _reloadCounter = reloadDuration;
    }
    
    private void _Initialize()
    {
        initialized = true;

        if (Gauge == null)
        {
            Gauge = GetComponent<MMRadialProgressBar>();
        }


        if (LevelManager.HasInstance)
        {
            WeaponHandler = LevelManager.Current.Players[0].GetComponent<CharacterHandleWeapon>();
        }
        else Debug.LogError("No Player found for ReloadGauge!");

        maxAmmo = WeaponHandler.CurrentWeapon.MagazineSize;
        reloadDuration = WeaponHandler.CurrentWeapon.ReloadTime + 0.05f;
    }

    private void Update()
    {
        if (!initialized) return;

        if (WeaponHandler.CurrentWeapon != null)
        {
            if (!reloading)
            {
                Gauge.UpdateBar(WeaponHandler.CurrentWeapon.CurrentAmmoLoaded, maxAmmo, 0f);
            }
            else
            {
                _reloadCounter -= Time.deltaTime;
                Gauge.UpdateBar(_reloadCounter, 0f, reloadDuration);

                if (_reloadCounter <= 0f)
                {
                    reloading = false;
                }
            }
        }
    }

    //implement event listener
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
        if (eventType.EventName.Equals("WeaponSwitched"))
        {
            _Initialize();
        }
        else if (eventType.EventName.Equals("ReloadStart"))
        {
            StartReload();
        }
    }
}
