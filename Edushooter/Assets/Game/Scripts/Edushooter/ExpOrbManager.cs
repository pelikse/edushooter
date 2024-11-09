using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpOrbManager : MMSingleton<ExpOrbManager>
{
    public MMObjectPooler ObjectPooler { get; set; }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (ObjectPooler != null) return;

        if (GetComponent<MMMultipleObjectPooler>() != null)
        {
            ObjectPooler = GetComponent<MMMultipleObjectPooler>();
        }
        if (GetComponent<MMSimpleObjectPooler>() != null)
        {
            ObjectPooler = GetComponent<MMSimpleObjectPooler>();
        }
        if (ObjectPooler == null)
        {
            Debug.LogWarning(this.name + " : no object pooler (simple or multiple) is attached to this ExpOrbManager, it won't be able to spawn anything.");
            return;
        }
    }

    // attempt to spawn an ExpOrb, used for when enemies die
    public void SpawnExpOrb(Vector3 position, float exp)
    {
        if (!Application.isPlaying) return;

        GameObject _orb = ObjectPooler.GetPooledGameObject();

        // mandatory checks
        if (_orb == null) { return; }
        if (_orb.GetComponent<MMPoolableObject>() == null)
        {
            throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
        }

        if (_orb.MMGetComponentNoAlloc<ExpOrb>() == null)
        {
            throw new Exception(gameObject.name + " isn't spawning an ExpOrb!");
        }

        // we activate the object
        _orb.gameObject.SetActive(true);
        _orb.gameObject.MMGetComponentNoAlloc<ExpOrb>().SetExpOrbValue(exp);
        _orb.gameObject.MMGetComponentNoAlloc<MMPoolableObject>().TriggerOnSpawnComplete();

        // we position the object
        _orb.transform.position = position;
    }
}
