using System.Collections.Generic;
using System;

using UnityEngine;

using MoreMountains.TopDownEngine;
using MoreMountains.Tools;



public class ExtendedTimedSpawner : TimedSpawner, MMEventListener<MMGameEvent>
{
    //implement singleton pattern here
    protected static ExtendedTimedSpawner _instance;
    public static bool HasInstance => _instance != null;
    public static ExtendedTimedSpawner TryGetInstance() => HasInstance ? _instance : null;
    public static ExtendedTimedSpawner Current => _instance;

    private void Awake()
    {
        _instance = this;
    }


    // attributes
    [Space]
    [Space]
    [Header("Extended Properties")]
    [Space]

    // a list of Transforms that are used as spawn positions
    [SerializeField]
    [MMReadOnly]
    protected List<SpawnerGroup> _spawnerList = new List<SpawnerGroup>();

    // toggle to true so the spawner cannot spawn on a timer and must be called (without turning off the spawner altogether)
    public bool ToggleSpawnTimer = false;

    [Space]

    /*
    // the player character, might need to be fixed later
    public Character player;
    public float minDistanceToPlayer = 30; //minimum distance that player can be from a spawner for it to spawn, 30 fits a fov of 50
    */

    // the next Transform to spawn at
    [SerializeField]
    [MMReadOnly]
    protected Transform _spawnPosition;

    [Space]

    // an int that tracks how many enemies have spawned
    [SerializeField]
    [MMReadOnly]
    private int enemiesSpawned = 0;

    // how many enemies that the timed spawner can spawn before it stops
    public int enemySpawnedLimit = 10;

    // how many enemies should be spawned together as a group in a single spot?
    [Space]
    [SerializeField]
    private int IdealGroupSize = 20;

    private int _groupSizeTracker = 0;
    private SpawnerGroup _spawnerGroup;

    [Space]

    public MMSimpleObjectPooler LastWaveBossPool;

    private bool _spawnCharger = false;


    // a limit for how many times the script should look for a fitting spawn, just a failsafe and probably should never be used
    //int SafeSpawnSearchLimit = 30;

    //overrides Start to initialize _spawnerList and sets next spawner
    protected override void Start()
    {
        Initialization();
        foreach (Transform child in transform) 
        {
            SpawnerGroup spawn = child.GetComponent<SpawnerGroup>();
            if (spawn != null)
            {
                _spawnerList.Add(spawn);
            }
        }

        // get the first spawn position from a spawn group
        _spawnerGroup = _spawnerList[UnityEngine.Random.Range(0, _spawnerList.Count)];
        _spawnPosition = _spawnerGroup.GetSpawnPoint();
    }

    // a public function to trigger spawn
    public void TrySpawn()
    {
        Spawn();
    }

    // reset the amount of enemy spawned that's tracked, should be refreshed per round
    public void ResetEnemySpawned()
    {
        enemiesSpawned = 0;
        _groupSizeTracker = 0;
    }

    // starts spawning enemies
    public void StartSpawning()
    {
        CanSpawn = true;
    }

    // overrides update to take into account ToggleSpawnTimer
    protected override void Update()
    {
        if ((Time.time - _lastSpawnTimestamp > _nextFrequency) && CanSpawn && ToggleSpawnTimer)
        {
            Spawn();
        }
    }

    private bool ShouldEndGroup(int size)
    {
        if (size < (int)(0.8f * IdealGroupSize))
        {
            return false;
        }
        else if (size > (int)(1.5f * IdealGroupSize))
        {
            return true;
        }
        else
        {
            return UnityEngine.Random.Range(0, (int)(1.5f * IdealGroupSize)) < (_groupSizeTracker - (int)(0.8f * IdealGroupSize));
        }
    }

    // sets the next spawn location
    public void SetNextSpawner()
    {
        // check if we should move spawn groups
        if (ShouldEndGroup(_groupSizeTracker))
        {
            //if so, then we reset the group size
            _groupSizeTracker = 0;

            // get the next spawn group from the spawner list
            _spawnerGroup = _spawnerList[UnityEngine.Random.Range(0, _spawnerList.Count)];
        }

        // set a spawn position from the currently selected spawn group
        _spawnPosition = _spawnerGroup.GetSpawnPoint();

        // add one to the group size
        _groupSizeTracker++;
    }

    /// A polymorph of Spawn that spawns at the desired transform
    protected override void Spawn()
    {
        GameObject nextGameObject;

        if (_spawnCharger)
        {
            nextGameObject = LastWaveBossPool.GetPooledGameObject();
            _spawnCharger = false;
        }
        else
        {
            nextGameObject = ObjectPooler.GetPooledGameObject();
        }

        SetNextSpawner();

        // mandatory checks
        if (nextGameObject == null) { return; }
        if (nextGameObject.GetComponent<MMPoolableObject>() == null)
        {
            throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
        }

        // we activate the object
        nextGameObject.gameObject.SetActive(true);
        nextGameObject.gameObject.MMGetComponentNoAlloc<MMPoolableObject>().TriggerOnSpawnComplete();

        // we check if our object has an Health component, and if yes, we revive our character
        Health objectHealth = nextGameObject.gameObject.MMGetComponentNoAlloc<Health>();
        if (objectHealth != null)
        {
            objectHealth.Revive();
        }

        // we position the object
        nextGameObject.transform.position = _spawnPosition.position;

        // increment enemiesSpawned and check if we should stop spawning
        enemiesSpawned++;
        if (enemiesSpawned >= enemySpawnedLimit)
        {
            //stops spawning and trigger event
            CanSpawn = false;
            MMGameEvent.Trigger("EnemySpawnLimitReached");
        }
        else
        {
            // we reset our timer and determine the next frequency
            _lastSpawnTimestamp = Time.time;
            DetermineNextFrequency();
        }
    }

    #region EventListener Implement
    // functions to implement event listener
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
        if (eventType.EventName == "LastWave")
        {
            _spawnCharger = true;
        }
    }
    #endregion
}
