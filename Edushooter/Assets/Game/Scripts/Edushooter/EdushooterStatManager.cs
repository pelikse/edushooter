using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdushooterStatManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    const float healthMultiplier = 1.25f;
    const float damageMultiplier = 0.8f;
    const float speedMultiplier = 0.3f;
    const float dashMultiplier = -0.3f;
    const float reloadMultiplier = -0.35f;

    [Header("Player Attributes")]
    public string PlayerID = "Player 1";
    PlayerEdushooterData StatData;

    //constants

    const float STARTING_MULTIPLIER = 1f;
    const float CURVE_MODIFIER = 0.5f;

    private float GetStatMultiplier(int level, float modifier)
    {
        return STARTING_MULTIPLIER + (CURVE_MODIFIER * Mathf.Sqrt(level) * modifier);
    }

    #region StatMultipliers

    public float GetHealthMultiplier(int level)
    {
        return GetStatMultiplier(level, healthMultiplier);
    }

    public float GetDamageMultiplier(int level)
    {
        return GetStatMultiplier(level, damageMultiplier);
    }

    public float GetSpeedMultiplier(int level)
    {
        return GetStatMultiplier(level, speedMultiplier);
    }

    public float GetDashMultiplier(int level)
    {
        return GetStatMultiplier(level, dashMultiplier);
    }

    public float GetReloadMultiplier(int level)
    {
        return GetStatMultiplier(level, reloadMultiplier);
    }

    #endregion

    // call this function to apply initial stat changes
    private void ApplyStatChanges()
    {
        // gets the first player in the list of players
        Character player = LevelManager.Current.Players[0];

        if (player == null)
        {
            Debug.LogError("No player character found in the scene when attempting to upgrade!");
            return;
        }

        // apply health changes
        if (player.gameObject.TryGetComponent<Health>(out var _playerHealth))
        {
            float _healthMultiplier = GetHealthMultiplier(StatData.Health);
            _playerHealth.CurrentHealth *= _healthMultiplier;
            _playerHealth.MaximumHealth *= _healthMultiplier;
            _playerHealth.InitialHealth *= _healthMultiplier;
        }

        // apply movement changes
        if (player.gameObject.TryGetComponent<CharacterMovement>(out var _playerMovement))
        {
            _playerMovement.WalkSpeed *= GetSpeedMultiplier(StatData.Speed);
            _playerMovement.ResetSpeed();
        }

        // apply dash cooldown changes
        if (player.gameObject.TryGetComponent<CharacterAbilityTeleport>(out var _playerDash))
        {
            _playerDash.CooldownDuration *= GetDashMultiplier(StatData.Dash);
        }

        // adjust damage of bullets and reload time
        if (player.gameObject.TryGetComponent<CharacterHandleWeapon>(out var _playerWeapon))
        {
            // reduce reload by stat level
            _playerWeapon.CurrentWeapon.ReloadTime *= GetReloadMultiplier(StatData.Reload);
           

            // assign damage stats to the bullet stat adjuster
            BulletDamageAdjuster adjuster = _playerWeapon.CurrentWeapon.GetComponent<BulletDamageAdjuster>();
            if (adjuster != null)
            {
                adjuster.AdjustBulletPoolDamage(GetDamageMultiplier(StatData.Damage));
            }
        }
    }

    private void Start()
    {
        StatData = ProgressManager.TryGetInstance().ExternalData.EdushooterStats;
    }

    #region EventListener Implement
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
        if (eventType.EventName.Equals("ApplyPlayerStats"))
        {
            ApplyStatChanges();
        }
    }

    #endregion
}
