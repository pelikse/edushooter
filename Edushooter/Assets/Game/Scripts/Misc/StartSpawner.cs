using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    public TimedSpawner Spawner;

    public float SpawnActiveDuration = 3f;

    private float _timerStart = 0f;
    private bool _spawnActive = false;

    public void StartSpawn()
    {
        Debug.Log("starting spawn");
        _timerStart = Time.time;
        _spawnActive = true;

        Spawner.CanSpawn = true;
    }

    private void Update()
    {
        if (_spawnActive)
        {
            //if we've exceeded the spawn duration, stop it
            if (Time.time - _timerStart >= SpawnActiveDuration)
            {
                Debug.Log("stopping spawn");
                _spawnActive = false;
                Spawner.CanSpawn = false;
            }
        }
    }
}
