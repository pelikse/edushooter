using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Tools;
using System;
using MoreMountains.TopDownEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class SessionManager : MMSingleton<SessionManager>, MMEventListener<MMGameEvent>
{
    [System.Serializable]
    public enum SessionState
    {
        Preparation,
        Ongoing,
        Intermission,
        Finished
    }

    [MMInformation("This manager takes care of starting sessions, managing waves/rounds of enemies, and tracking enemy numbers during gameplay.", MMInformationAttribute.InformationType.Info, true)]

    [Header("Session Attributes")]
    public bool CanStartSession = true;

    [SerializeField]
    [MMReadOnly]
    private SessionState CurrentSessionState = SessionState.Preparation;

    [Space]
    // how many rounds should the manager play?
    public int RoundLimit = 5;
    public float RoundIntermissionDuration = 10f;

    [Space]

    [Header("UI Displays")]
    // the UI panel to allow players to adjust difficulty
    public GameObject DifficultyAdjustPanel;

    [Space, Space]
    [Header("Wave Attributes")]
    public int InitialEnemySpawnLimit = 12;
    public float RoundMultiplier = 1.5f;    

    [Space, Space, Space]

    [Header("Debugging")]

    [SerializeField]
    [MMReadOnly]
    private int _enemySpawnLimit = 0;

    [SerializeField]
    [MMReadOnly]
    private int Rounds = 0;

    [SerializeField]
    [MMReadOnly]
    private int EnemiesLeft = 0;

    [Space]

    [SerializeField]
    [MMReadOnly]
    private int coinReward = 10;

    [Space,Space]

    [SerializeField]
    [MMReadOnly]
    private float _intermissionTime = 0f;

    // private attributes
    private float _lastIntermissionTimestamp;
    private bool _intermissionEnable = false;

    public int CoinReward { get => coinReward; set => coinReward = value; }

    private int _enemiesKilled = 0;

    private const float END_SESSION_DELAY = 2.5f;

    public int GetEnemiesKilled()
    {
        return _enemiesKilled;
    }

    private void EnemyDied()
    {
        // number of enemies left is decremented and updated in the UI
        EnemiesLeft--;
        _enemiesKilled++;

        RefreshEnemiesLeft();

        if (EnemiesLeft == 0)
        {
            // if all enemies have died, start intermission if there's a next round
            if (Rounds < RoundLimit)
            {
                SwitchSessionState(SessionState.Intermission);
            }
            // if there's no more rounds, then end the game
            else if (Rounds >= RoundLimit)
            {
                Debug.Log("all enemies killed, ending session...");
                StartCoroutine(EndCurrentSession());
            }
        }
    }

    public void AddSessionCoinRewards(int extraCoins)
    {
        CoinReward += extraCoins;
    }

    private IEnumerator EndCurrentSession()
    {
        Debug.Log("starting coroutine");
        MMGameEvent.Trigger("ToggleEndText");

        //wait for a few seconds
        yield return new WaitForSeconds(3);

        Debug.Log("trigger session finish");
        SwitchSessionState(SessionState.Finished);
        MMGameEvent.Trigger("SessionFinished");
        MMGameEvent.Trigger("TurnOffHUD");
    }

    public void SwitchSessionState(SessionState state)
    {
        switch (state)
        {
            //entering preparation, immediately start the game
            case SessionState.Preparation:
                DifficultyAdjustPanel?.SetActive(true);

                MMTimeManager.TryGetInstance().SetTimeScaleTo(1f);
                MMGameEvent.Trigger("UnpauseSession");
                break;

            //every time we switch back to the ongoing round, if we're not on the first round then execute startnewround
            case SessionState.Ongoing:
                MMGameEvent.Trigger("WaveStart");

                if (Rounds > 0) StartNewRound();
                else GUIManager.Current.HUD.SetActive(true);

                break;

            //intermission starts, we trigger the intermission
            case SessionState.Intermission:
                _lastIntermissionTimestamp = Time.time;
                _intermissionEnable = true;

                MMGameEvent.Trigger("IntermissionStart");
                break;

            //the end of the session, pause time and cannot go anywhere again
            case SessionState.Finished:
                //reward the player with gems
                ProgressManager.TryGetInstance().AddCoins(CoinReward);

                //set shuffled map so we can get new maps
                ProgressManager.TryGetInstance().SetShuffledMapPlayed(true);

                MMGameEvent.Trigger("TriggerSave");
                break;
        } 
        
        CurrentSessionState = state;
    }
    
    private void RefreshEnemiesLeft()
    {
        MMSingleton<RoundHUDManager>.TryGetInstance().SetEnemiesLeft(EnemiesLeft);
    }

    private void IncrementRound()
    {
        Rounds++;
        MMSingleton<RoundHUDManager>.TryGetInstance().SetRoundsPassed(Rounds);
    }

    // starts the first round
    private void StartFirstRound()
    {
        // the session is now ongoing
        SwitchSessionState(SessionState.Ongoing);

        // increments the round tracker
        IncrementRound();

        // gets the spawners
        if (ExtendedTimedSpawner.TryGetInstance() == null)
        {
            throw new Exception("There is no instance of ExtendedTimedSpawner for the RoundManager to use!");
        }

        // reset the number of spawned enemy and sets the spawn limit
        ExtendedTimedSpawner.Current.ResetEnemySpawned();
        ExtendedTimedSpawner.Current.enemySpawnedLimit = _enemySpawnLimit;

        // reset the number of enemies left that sessionmanager tracks
        EnemiesLeft = _enemySpawnLimit;
        RefreshEnemiesLeft ();

        // start spawning
        ExtendedTimedSpawner.Current.StartSpawning();
    }

    //starts a new round by resetting enemy spawned and setting a new enemy spawn limit
    private void StartNewRound()
    {
        IncrementRound();

        // if this is the last round...
        // and the difficulty is 3 or greater ...
        // spawn the charger
        if (Rounds == RoundLimit && DifficultyManager.TryGetInstance().difficultyLevel > 3)
        {
            // spawn the charger, add +1 to the spawn limit
            MMGameEvent.Trigger("LastWave");
        }

        ExtendedTimedSpawner.Current.ResetEnemySpawned();
        _enemySpawnLimit = Mathf.RoundToInt(_enemySpawnLimit * RoundMultiplier);
        ExtendedTimedSpawner.Current.enemySpawnedLimit = _enemySpawnLimit;

        EnemiesLeft = _enemySpawnLimit;
        RefreshEnemiesLeft ();
        ExtendedTimedSpawner.Current.StartSpawning();
    }

    // call the function when the player is ready to play and has picked their difficulty
    public void EndPreparationState()
    {
        // tries to get the difficulty manager
        if (DifficultyManager.TryGetInstance() != null)
        {
            //adjusts the difficulty level that enemies use to update their levels
            DifficultyManager.Instance.AdjustDifficultyLevel(ProgressManager.TryGetInstance().LocalCache.DifficultyLevel);

            //adjust the enemy spawning limit
            InitialEnemySpawnLimit = (int)Mathf.Ceil(InitialEnemySpawnLimit * DifficultyManager.Instance.InitialEnemyMultiplier);
            Debug.Log("initial enemy is " + InitialEnemySpawnLimit);
            RoundMultiplier *= DifficultyManager.Instance.WaveMultiplier;
            Debug.Log("round multiplier is " + RoundMultiplier);
        }

        // sets the first round's spawn limit
        _enemySpawnLimit = InitialEnemySpawnLimit;

        // make sure time scale is ok
        MMTimeManager.TryGetInstance().SetTimeScaleTo(1f);
        MMGameEvent.Trigger("UnpauseSession");

        Time.timeScale = 1f;
        Debug.Log("time scale: " + Time.timeScale);

        // sends an event to start the count down
        if (CanStartSession) MMGameEvent.Trigger("StartSessionCountdown");
    }

    public void SetCoinReward(int coins)
    {
        CoinReward = coins;
    }



    private void Start()
    {
        SwitchSessionState(SessionState.Preparation);
        _enemiesKilled = 0;
    }

    private void Update()
    {
        // if we're in intermission
        if (_intermissionEnable)
        {
            // track intermission time
            _intermissionTime = Time.time - _lastIntermissionTimestamp;

            // if we've exceeded the intermission duration
            if (_intermissionTime > RoundIntermissionDuration)
            {
                _intermissionEnable = false;
                SwitchSessionState(SessionState.Ongoing);
            }
        }
    }


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
            case "EnemyDied":
                EnemyDied();
                break;

            // the signal that countdown has ended and we're ready to start spawning
            case "StartSessionGameplay":
                StartFirstRound();
                break;

            // listen to the signal of the end of preparation, start countdown
            case "EndSessionPrep":
                EndPreparationState();
                break;
        }
    }
    #endregion
}
