using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabAdjuster : MonoBehaviour
{
    //[MMInformation("This manager changes the enemy prefab stats directly AND PERMANENTLY. Should be a last resort for adjusting stats if it's too CPU Intensive to adjust stats on the fly.", MMInformationAttribute.InformationType.Warning, true)]
    //[SerializeField] private bool CanAdjustPrefab = false;

    //[Space]

    //[SerializeField] private StatAdjuster[] EnemyPrefabs;
    //[SerializeField] private DifficultyManager DifficultyManager;

    //[Space]

    //[MMInspectorButton("TestAdjustStats")]
    /// A test button to test adding coins
    //public bool TestAddCoinsBtn;
    //protected virtual void TestAdjustStats()
    //{
    //    AdjustAllStats();
    //}

    //public void AdjustAllStats()
    //{
    //    if (!CanAdjustPrefab) return;

    //    //enforce the stat adjustments on all prefabs
    //    foreach (StatAdjuster prefab in EnemyPrefabs)
    //    {
    //        if (prefab != null)
    //        {
    //            prefab.TryAdjust(DifficultyManager);
    //        }
    //    }
    //}

    //Start is called before the first frame update
    //void Start()
    //{
    //    AdjustAllStats();
    //}
}
