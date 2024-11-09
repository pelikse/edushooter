using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks/Coin Perk")]
public class ExtraCoinPerk : PerkScriptable
{
    public int extraCoinRewards = 10;

    public override void ApplyPerkEffect()
    {
        // gets the first player in the list of players
        if (SessionManager.Instance != null)
        {
            SessionManager.Instance.AddSessionCoinRewards(extraCoinRewards);
        }
    }
}
