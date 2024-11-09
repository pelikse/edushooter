using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerStatAdjuster : StatAdjuster
{
    [Space, Space]

    public Health EntityHP;
    public CharacterMovement EntityMovement;
    public override void AdjustStats(DifficultyManager m)
    {
        float _hpMultiplier = m.HealthMultiplier;

        EntityHP.MaximumHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.InitialHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.CurrentHealth = Stats.HitPoints * _hpMultiplier;

        if (Stats.AdjustSpeed)
        {
            EntityMovement.WalkSpeed = Stats.Speed * m.SpeedMultiplier;
            EntityMovement.ResetSpeed();
        }

        Debug.Log("charger stat adjusted with difficulty level " + m.difficultyLevel);
    }

    public void AdjustStats()
    {
        if (Adjusted) return;

        DifficultyManager m = DifficultyManager.TryGetInstance();

        float _hpMultiplier = m.HealthMultiplier;

        EntityHP.MaximumHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.InitialHealth = Stats.HitPoints * _hpMultiplier;
        EntityHP.CurrentHealth = Stats.HitPoints * _hpMultiplier;

        if (Stats.AdjustSpeed)
        {
            EntityMovement.WalkSpeed = Stats.Speed * m.SpeedMultiplier;
            EntityMovement.ResetSpeed();
        }

        Adjusted = true;
    }
}
