using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MedicStatAdjuster : StatAdjuster
{
    [Space, Space]
    [Header("Custom Stats")]
    public MedicStatistics MedicStats;

    [Space]

    public Health EntityHP;
    public CharacterMovement EntityMovement;
    public MedicAura MedicAura;

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
        Debug.Log(MedicStats.BaseResistance * m.ResistanceMultiplier);
        MedicAura.DamageReduction = 1f - (MedicStats.BaseResistance * m.ResistanceMultiplier);

        MedicAura.HealingAmount = MedicStats.BaseHealing * m.HealingMultiplier;

        Debug.Log("medic prefab stat adjusted with difficulty level " + m.difficultyLevel);
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
        Debug.Log(MedicStats.BaseResistance * m.ResistanceMultiplier);
        MedicAura.DamageReduction = 1f - (MedicStats.BaseResistance * m.ResistanceMultiplier);

        MedicAura.HealingAmount = MedicStats.BaseHealing * m.HealingMultiplier;

        Adjusted = true;
    }
}
