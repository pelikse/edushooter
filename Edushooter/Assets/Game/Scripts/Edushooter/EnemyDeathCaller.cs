using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCaller : MonoBehaviour
{

    public Transform self;

    void Start()
    {
        if (self == null)
        {
            self = gameObject.transform;
        }
    }

    // kalau mati panggil explosion berdasarkan lokasi enemy death
    public void CallDeathEffect()
    {
        ExplosionPoolManager.TryGetInstance()?.SpawnExplosion(self.position);
    }
}
