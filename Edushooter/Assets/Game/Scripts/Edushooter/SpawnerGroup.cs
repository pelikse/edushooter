using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerGroup : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Spawns;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Spawns.Add(child);
        }
    }

    public Transform GetSpawnPoint()
    {
        // if there's no child spawn points then return self
        if (Spawns.Count == 0) return transform;

        return Spawns[Random.Range(0, Spawns.Count)];
    }
}
