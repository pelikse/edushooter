using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatAdjuster : MonoBehaviour
{
    public EnemyStatistics Stats;

    [SerializeField]
    [MMReadOnly]
    protected bool Adjusted = false;

    public abstract void AdjustStats(DifficultyManager m);
}
