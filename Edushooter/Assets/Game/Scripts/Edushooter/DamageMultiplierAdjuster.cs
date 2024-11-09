using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplierAdjuster : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [MMInformation("Every time the Player is enabled, it adjusts the DamageMultiplier stat according to the difficulty.", MMInformationAttribute.InformationType.None, false)]

    [SerializeField] private PlayerHealth PlayerHealth;

    public void AdjustDamageTaken()
    {
        DifficultyManager diff = DifficultyManager.TryGetInstance();

        if (diff != null )
        {
            PlayerHealth.DamageMultiplier = diff.DamageMultiplier;
        }
        else
        {
            PlayerHealth.DamageMultiplier = 1;
        }
        
    }

    protected virtual void OnEnable()
    {
        AdjustDamageTaken();
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        //when countdown starts, we should adjust the stats
        if (eventType.EventName.Equals("StartSessionCountdown"))
        {
            AdjustDamageTaken();
        }
    }
}
