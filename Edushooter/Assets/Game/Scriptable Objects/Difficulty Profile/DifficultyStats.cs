using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyOption", menuName = "Difficulty Statistics")]
public class DifficultyStats : ScriptableObject
{
    // each of these stats will be multiplied by the level of the current game session
    // in percentages
    public float HealthMultiplier = 5f;
    public float DamageMultiplier = 2.5f;
    public float SpeedMultiplier = 0.5f;
    public float InitialEnemyMultiplier = 5f;
    public float WaveMultiplier = 0.25f;
}
