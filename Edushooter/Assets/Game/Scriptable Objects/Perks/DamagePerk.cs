using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks/Damage Perk")]
public class DamagePerk : PerkScriptable
{
    public float damageMultiplier = 1.15f;

    public override void ApplyPerkEffect()
    {
        // gets the first player in the list of players
        Character player = LevelManager.Current.Players[0];

        // adjust damage of bullets and reload time
        if (player.gameObject.TryGetComponent<CharacterHandleWeapon>(out var _playerWeapon))
        {
            // assign damage stats to the bullet stat adjuster
            BulletDamageAdjuster adjuster = _playerWeapon.CurrentWeapon.GetComponent<BulletDamageAdjuster>();
            if (adjuster != null)
            {
                adjuster.AdjustBulletPoolDamage(damageMultiplier);
            }
        }
    }
}
