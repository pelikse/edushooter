using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks/Shield Perk")]
public class ShieldPerk : PerkScriptable
{
    public short shieldAmount = 1;

    public override void ApplyPerkEffect()
    {
        // gets the first player in the list of players
        Character player = LevelManager.Current.Players[0];

        if (player.gameObject.TryGetComponent<PlayerHealth>(out var _playerHealth))
        {
            _playerHealth.AddArmor(shieldAmount);
        }
    }
}
