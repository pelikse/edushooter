using HighlightPlus;
using MoreMountains.TopDownEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Regeneration")]
public class RegenerationCondition : PowerupCondition
{
    [Space,Space]
    [Header("Regen Attributes")]
    public float HealthPercentageToRecover;

    private Health ownerHealth;
    private float healthToRecover;

    public override void StartCondition(GameObject Owner)
    {
        Debug.Log(Owner.gameObject.name);
        
        if (Owner.TryGetComponent<Health>(out ownerHealth))
        {
            healthToRecover = Mathf.Floor(ownerHealth.MaximumHealth * HealthPercentageToRecover * 0.01f);

            Debug.Log("starting regeneration...");
        }
    }

    public override void EndCondition()
    {
        Debug.Log("ending regeneration...");
    }

    public override void RepeatableConditionEffect()
    {
        ownerHealth.ReceiveHealth(healthToRecover, ownerHealth.gameObject);
    }
}
