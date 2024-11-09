using MoreMountains.Tools;
using System;
using UnityEngine;

public class HealingOrbManager : MMSingleton<HealingOrbManager>
{
    [Header("Healing Orb Pool")]
    public MMSimpleObjectPooler HealingOrbPool;

    [Space, Space]

    [Header("Healing Drop Attributes")]
    public float MinimumCooldown = 5f;
    public float MaximumCooldown = 8f;

    [Space]

    [MMInformation("DropProbability should be between 0f - 1f, it represents the chance of a successful drop of a healing orb when an enemy is killed.", MMInformationAttribute.InformationType.Info, true)]
    public float DropProbability = 0.15f;

    [Space]

    public float LowHealthThreshold = 0.25f;
    public float LowHealthMultiplier = 2f;

    //private attributes
    [Space,Space]

    [Header("Debugging")]
    private float lastSpawnTime;

    [SerializeField]
    [MMReadOnly]
    private float spawnCooldown;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    public void TryDropHealing(Vector3 position)
    {
        //if we haven't exceeded our cooldown, then don't spawn
        if ((Time.time - lastSpawnTime) < spawnCooldown) return;

        //check if we should drop healing based on probability
        if (UnityEngine.Random.Range(0f, 1f) < DropProbability)
        {
            SpawnHealingOrb(position);
        }
    }
    private void SpawnHealingOrb(Vector3 spawnPosition)
    {
        if (HealingOrbPool != null)
        {
            GameObject nextGameObject = HealingOrbPool.GetPooledGameObject();

            // mandatory checks
            if (nextGameObject == null) { return; }
            if (nextGameObject.GetComponent<MMPoolableObject>() == null)
            {
                throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
            }

            // we activate the object
            nextGameObject.gameObject.SetActive(true);
            nextGameObject.gameObject.MMGetComponentNoAlloc<MMPoolableObject>().TriggerOnSpawnComplete();

            // we position the object
            nextGameObject.transform.position = spawnPosition;

            // start the cooldown
            lastSpawnTime = Time.time;
            spawnCooldown = UnityEngine.Random.Range(MinimumCooldown, MaximumCooldown);
        }
    }

}
