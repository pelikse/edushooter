using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyEntry", menuName = "EnemyEntry")]
public class EnemyDirectoryEntry : ScriptableObject
{
    [System.Serializable]
    public enum StatMagnitude
    {
        None,
        Low,
        Moderate,
        High,
        Extreme,
    }

    public EnemyModelSwitch.ModelType Enemy;

    [Space]

    public StatMagnitude Health;
    public StatMagnitude Damage;
    public StatMagnitude Speed;

    [Space]
    [Space]

    [TextArea(4, 6)]
    public string EnemyDescription;

    [Space]

    [TextArea(3, 5)]
    public string EnemyTips;
}
