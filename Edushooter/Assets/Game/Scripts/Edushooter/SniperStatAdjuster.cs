using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperStatAdjuster : StatAdjuster
{
    public Health EntityHP;
    public CharacterMovement EntityMovement;

    public override void AdjustStats(DifficultyManager m)
    {

    }

    public void AdjustStats()
    {
        if (Adjusted) return;

        DifficultyManager m = DifficultyManager.TryGetInstance();

        float _hpMultiplier = m.HealthMultiplier;

        EntityHP.MaximumHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.InitialHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.CurrentHealth = Stats.HitPoints * _hpMultiplier;


        EntityMovement.WalkSpeed = Stats.Speed * m.SpeedMultiplier;
        EntityMovement.ResetSpeed();

        Adjusted = true;
    }
}
