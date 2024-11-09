using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAdjuster : MonoBehaviour
{
    public DamageOnTouch BulletDamage;

    public void MultiplyDamage(float multiplier)
    {
        BulletDamage.MinDamageCaused *= multiplier;
        BulletDamage.MaxDamageCaused *= multiplier;

        foreach (TypedDamage dmg in BulletDamage.TypedDamages)
        {
            dmg.MinDamageCaused *= multiplier;
            dmg.MaxDamageCaused *= multiplier;
        }
    }
}
