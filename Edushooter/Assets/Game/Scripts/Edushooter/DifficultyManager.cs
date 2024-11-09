using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MMSingleton<DifficultyManager>
{
    [MMInformation("Difficuly Level provides scalable enemy stats and wave sizes according to the level of difficulty.", MMInformationAttribute.InformationType.None, false)]
    [Header("Difficulty Attributes")]

    public int difficultyLevel = 1;

    //these multipliers are universal
    //enemy stats
    private float finalHealthMultiplier = 3f;
    private float finalDamageMultiplier = 1.75f;
    private float finalSpeedMultiplier = 1.35f;

    //wave stats
    private float finalInitialEnemyMultiplier = 1.5f;
    private float finalWaveMultiplier = 1.25f;

    //custom stats for specific enemies
    private float finalHealingMultiplier = 1.5f;
    private float finalResistanceMultiplier = 2f;


    public void AdjustDifficultyLevel(int difficultyLevel)
    {
        this.difficultyLevel = difficultyLevel;
    }

#region MultiplierDefinitions
    public float HealthMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalHealthMultiplier);}
    public float DamageMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalDamageMultiplier); }
    public float SpeedMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalSpeedMultiplier);}
    public float InitialEnemyMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalInitialEnemyMultiplier);}
    public float WaveMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalWaveMultiplier);}
    public int DifficultyLevel { get => difficultyLevel; set => difficultyLevel = value; }
    public float HealingMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalHealingMultiplier); }
    public float ResistanceMultiplier { get => DifficultyPanel.GetDifficultyModifier(difficultyLevel, finalResistanceMultiplier); }
#endregion
}
