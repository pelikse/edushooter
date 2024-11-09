using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks/Armor Perk")]
public class ArmorPerk : PerkScriptable
{
    [MMInformation("DamageReduction should NEVER BE OVER 1f! It's treated as a percentage so 1f means that its 100% damage reduction! Keep it to 0.2f max!", MMInformationAttribute.InformationType.Info, true)]
    public float damageReduction = 0.1f;

    public override void ApplyPerkEffect()
    {
        // gets the first player in the list of players
        Character player = LevelManager.Current.Players[0];

        // adjust the player's damage multiplier, reducing it
        if (player.gameObject.TryGetComponent<PlayerHealth>(out var _playerHealth))
        {
            _playerHealth.DamageMultiplier *= (1-damageReduction);
        }
    }
}
