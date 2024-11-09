using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks/Speed Perk")]
public class SpeedPerk : PerkScriptable
{
    public float SpeedIncrease;

    public override void ApplyPerkEffect()
    {
        // gets the first player in the list of players
        Character player = LevelManager.Current.Players[0];

        if (player.gameObject.TryGetComponent<CharacterMovement>(out var _playerMovement))
        {
            //multiply the player's movement speed and reset it
            _playerMovement.WalkSpeed += SpeedIncrease;
            _playerMovement.ResetSpeed();
        }
    }
}
