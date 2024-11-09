using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PowerupSpawner : MMSingleton<PowerupSpawner>, MMEventListener<MMGameEvent>
{
    public MMMultipleObjectPooler pool;
    public List<Transform> predefinedSpawnPositions = new List<Transform>();

    private bool hasSpawned = false;
    private GameObject currentPowerup;

    #region EventListener Implement
    private void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        switch (eventType.EventName)
        {
            case "WaveStart":
                if (currentPowerup != null && currentPowerup.activeInHierarchy)
                {
                    Debug.Log("Deactivating previous powerup before new wave starts...");
                    PowerupIndicatorManager.TryGetInstance()?.RemovePowerup(currentPowerup);
                    currentPowerup.SetActive(false);
                    currentPowerup = null;
                }

                hasSpawned = false;
                SpawnObject();
                break;

            case "WaveEnd":
                if (currentPowerup != null && currentPowerup.activeInHierarchy)
                {
                    Debug.Log("Powerup not collected, deactivating...");
                    PowerupIndicatorManager.TryGetInstance()?.RemovePowerup(currentPowerup);
                    currentPowerup.SetActive(false);
                    currentPowerup = null;
                }
                break;
        }
    }
    #endregion

    public void SpawnObject()
    {
        if (hasSpawned)
        {
            return;
        }

        Vector3 playerPosition = LevelManager.Instance.Players[0].transform.position;

        var sortedPositions = predefinedSpawnPositions
            .OrderByDescending(spawnPosition => Vector3.Distance(playerPosition, spawnPosition.position))
            .Take(3)
            .ToList();

        Debug.Log("Three furthest positions:");
        foreach (var pos in sortedPositions)
        {
            Debug.Log($"Position: {pos.position}, Distance: {Vector3.Distance(playerPosition, pos.position)}");
        }

        if (sortedPositions.Count > 0)
        {
            Transform selectedPosition = sortedPositions[Random.Range(0, sortedPositions.Count)];

            Debug.Log($"Randomly selected position: {selectedPosition.position}");

            GameObject pooledObject = pool.GetPooledGameObject();
            if (pooledObject != null)
            {
                pooledObject.transform.position = selectedPosition.position;
                PowerupIndicatorManager.TryGetInstance()?.AddActivePowerup(pooledObject);
                pooledObject.SetActive(true);
                hasSpawned = true;
                currentPowerup = pooledObject;
            }
        }
    }
}
