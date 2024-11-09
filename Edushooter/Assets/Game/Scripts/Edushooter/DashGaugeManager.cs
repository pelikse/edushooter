using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashGaugeManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public MMRadialProgressBar Gauge;
    public TextMeshProUGUI Charges;
    [Space]
    [SerializeField]
    [MMReadOnly]
    protected CharacterAbilityTeleport CharacterDash;

    protected float cooldownDuration;
    protected float _cooldownCounter;

    private bool _recharging = false;

    private bool initialized = false;

    // attempt a dash, this initiates the dash gauge
    public void StartDashing()
    {
        // if the dash isn't in total cooldown then the teleport succeeds
        if (CharacterDash.AbilityState == CharacterAbilityTeleport.TeleportAbilityState.Cooldown) return;

        cooldownDuration = CharacterDash.CooldownDuration + 0.05f;

        _recharging = true;

        _cooldownCounter = 0;
        Charges.text = (CharacterDash.ChargesRemaining - 1).ToString();
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
            CharacterDash = LevelManager.Current.Players[0].GetComponent<CharacterAbilityTeleport>();
        }
        else Debug.LogError("No Player found for ReloadGauge!");

        cooldownDuration = CharacterDash.CooldownDuration;
    }

    private void Update()
    {
        if (!initialized) return;

        //if there's a dash ability
        if (CharacterDash != null && _recharging)
        {
            _cooldownCounter += Time.deltaTime;

            Gauge.UpdateBar(_cooldownCounter, 0, cooldownDuration);

            if (_cooldownCounter >= cooldownDuration)
            {
                _recharging = false;
                Gauge.UpdateBar(cooldownDuration, 0, cooldownDuration);
                Charges.text = CharacterDash.ChargesRemaining.ToString();
            }
        }
    }

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
        if (eventType.EventName.Equals("StartSessionCountdown") || eventType.EventName.Equals("InitializeDashGauge"))
        {
            _Initialize();
        }
    }
}
