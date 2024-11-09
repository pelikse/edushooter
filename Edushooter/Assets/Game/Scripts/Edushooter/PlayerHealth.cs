using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event triggered every time health values change, for other classes to listen to
/// </summary>
public struct HealthChangeEvent
{
    public Health AffectedHealth;
    public float NewHealth;

    public HealthChangeEvent(Health affectedHealth, float newHealth)
    {
        AffectedHealth = affectedHealth;
        NewHealth = newHealth;
    }

    static HealthChangeEvent e;
    public static void Trigger(Health affectedHealth, float newHealth)
    {
        e.AffectedHealth = affectedHealth;
        e.NewHealth = newHealth;
        MMEventManager.TriggerEvent(e);
    }
}

public class PlayerHealth : Health
{
    [MMInspectorGroup("Extensions", true, 15)]
    [MMInformation("The multiplier that all damage the Character receives will be multiplied with.", MMInformationAttribute.InformationType.Info, false)]
    /// the target animator to pass a Death animation parameter to. The Health component will try to auto bind this if left empty
    [Tooltip("the multiplier that all damage received will be multiplied with.")]
    public float DamageMultiplier;

    [MMInformation("The health at which the player will be left with when hit with a killing blow once per life.", MMInformationAttribute.InformationType.Info, false)]
    [SerializeField]
    [MMReadOnly]
    private bool Sturdy = true;

    [SerializeField]
    private float SturdyHealth = 5f;

    [MMInspectorGroup("Armor", true, 16)]

    [MMInformation("The number of damage instances that this character can ignore.", MMInformationAttribute.InformationType.Info, false)]
    [SerializeField]
    private short Armor = 0;

    const short ARMOR_CAP = 5;

    public override float ComputeDamageOutput(float damage, List<TypedDamage> typedDamages = null, bool damageApplied = false)
    {
        if (Invulnerable || ImmuneToDamage)
        {
            return 0;
        }

        //if you have armor, reduce damage to 0
        if (Armor > 0)
        {
            Armor--;
            if (Armor <= 0)
            {
                MMGameEvent.Trigger("ArmorBreak");
            }
            else
            {
                MMGameEvent.Trigger("ArmorDamage");
            }
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

        float appliedDamage = totalDamage * DamageMultiplier;

        //if we still have sturdy
        if (Sturdy)
        {
            //if this damage will kill the player
            if (CurrentHealth - appliedDamage <= 0f)
            {
                //set it so that the player will be left with a sliver of health
                appliedDamage = Mathf.Clamp(CurrentHealth - SturdyHealth, 1f, appliedDamage);

                //turn off sturdy
                Sturdy = false;
            }
        }

        return appliedDamage;
    }

    public void AddArmor(int amount)
    {
        if (Armor == 0)
        {
            MMGameEvent.Trigger("ArmorCharge");
        }

        Armor = (short)Mathf.Clamp(Armor + amount, 0, ARMOR_CAP);
    }
}
