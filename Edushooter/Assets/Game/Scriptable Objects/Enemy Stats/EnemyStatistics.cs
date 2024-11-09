using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomObjects", menuName = "EnemyStats")]
public class EnemyStatistics : ScriptableObject
{
    public float HitPoints;
    //public float MinDamage;
    //public float MaxDamage;
    public float Speed;

    [Space]
    [Space]

    public bool AdjustSpeed = false;
}
