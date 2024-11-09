using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipper : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [Header("Weapon Prefabs")]
    public Weapon AssaultRiflePrefab;
    public Weapon ShotgunPrefab;
    public Weapon SmgPrefab;
    public Weapon SniperPrefab;
    public Weapon GatlingPrefab;
    public Weapon RPGPrefab;
    public Weapon FlamethrowerPrefab;

    [Space,Space]

    public CharacterHandleWeapon weaponHandler;



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
        if (eventType.EventName == "EndSessionPrep")
        {
            WeaponType type = ProgressManager.TryGetInstance().LocalCache.EquippedWeapon;
            Debug.Log(type);
            switch (type)
            {
                case WeaponType.AssaultRifle:
                    weaponHandler.ChangeWeapon(AssaultRiflePrefab, AssaultRiflePrefab.WeaponID);
                    break;

                case WeaponType.Shotgun:
                    weaponHandler.ChangeWeapon(ShotgunPrefab, ShotgunPrefab.WeaponID);
                    break;
                case WeaponType.Sniper:
                    weaponHandler.ChangeWeapon(SniperPrefab, SniperPrefab.WeaponID);
                    break;
                    
                case WeaponType.Smg:
                    weaponHandler.ChangeWeapon(SmgPrefab, SmgPrefab.WeaponID);
                    break;

                case WeaponType.Gatling:
                    weaponHandler.ChangeWeapon(GatlingPrefab, GatlingPrefab.WeaponID);
                    break;

                case WeaponType.Rpg:
                    weaponHandler.ChangeWeapon(RPGPrefab, RPGPrefab.WeaponID);
                    break;

                case WeaponType.Flamethrower:
                    weaponHandler.ChangeWeapon(FlamethrowerPrefab, FlamethrowerPrefab.WeaponID);
                    break;

                default:
                    Debug.LogWarning("something went wrong when equipping weapon, setting to assault rifle!");
                    weaponHandler.ChangeWeapon(AssaultRiflePrefab, AssaultRiflePrefab.WeaponID);
                    break;
            }

            MMGameEvent.Trigger("WeaponSwitched");
        }
    }
}
