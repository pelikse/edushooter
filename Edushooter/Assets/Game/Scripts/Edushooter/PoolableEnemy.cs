using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableEnemy : MMPoolableObject
{
    [Space]
    [Space]
    public float ExpToGive = 2f;

    public override void Destroy()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MMSingleton<ExpOrbManager>.TryGetInstance() == null)
        {
            throw new System.Exception("ExpOrbManager isn't found in the current scene!");
        }
    }

    public void OnDeathSpawnOrb()
    {
        // only spawn exp orb if the game is playing
        if (!Application.isPlaying)
        {
            return;
        }
        ExpOrbManager.Instance.SpawnExpOrb(transform.position, ExpToGive);
    }
}
