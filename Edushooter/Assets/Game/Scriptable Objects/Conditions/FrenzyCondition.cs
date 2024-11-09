using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Frenzy")]
public class FrenzyCondition : PowerupCondition
{
    public float AtkSpdMultiplier = 2f;

    private Weapon equippedWeapon;
    private float initialTimeBetweenShot;

    public override void StartCondition(GameObject Owner)
    {
        Debug.Log("fire rate increased by " + AtkSpdMultiplier);
        equippedWeapon = Owner.GetComponent<CharacterHandleWeapon>().CurrentWeapon;

        //reduce the time between shots
        if (equippedWeapon != null )
        {
            initialTimeBetweenShot = equippedWeapon.TimeBetweenUses;

            equippedWeapon.TimeBetweenUses = initialTimeBetweenShot / AtkSpdMultiplier;
        }
    }

    public override void EndCondition()
    {
        if (equippedWeapon != null)
        {
            Debug.Log("fire rate returned to normal");
            equippedWeapon.TimeBetweenUses = initialTimeBetweenShot;
        }
    }

    public override void RepeatableConditionEffect()
    {
        throw new System.NotImplementedException();
    }
}
