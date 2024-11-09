using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    [MMInspectorGroup("Damage Multiplier", true, 20)]
    /// the target animator to pass a Death animation parameter to. The Health component will try to auto bind this if left empty
    [Tooltip("the amount of multiplier all damage should be multiplied by, defaults to 1")]
    public float DamageMultiplier = 1f;

    public override float ComputeDamageOutput(float damage, List<TypedDamage> typedDamages = null, bool damageApplied = false)
    {
        if (Invulnerable || ImmuneToDamage)
        {
            return 0;
        }

        float totalDamage = 0f;
        // we process our damage through our potential resistances
        if (TargetDamageResistanceProcessor != null)
        {
            if (TargetDamageResistanceProcessor.isActiveAndEnabled)
            {
                totalDamage = TargetDamageResistanceProcessor.ProcessDamage(damage, typedDamages, damageApplied);
            }
        }
        else
        {
            totalDamage = damage;
            if (typedDamages != null)
            {
                foreach (TypedDamage typedDamage in typedDamages)
                {
                    totalDamage += typedDamage.DamageCaused;
                }
            }
        }

        totalDamage *= DamageMultiplier;

        return totalDamage * DamageMultiplier;
    }
}
