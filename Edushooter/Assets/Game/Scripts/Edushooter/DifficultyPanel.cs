using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPanel : MonoBehaviour
{
    [Space]
    [Header("Difficulty Stats")]
    public short DifficultyLevel = 0;
    public DifficultyLevelFetcher DifficultyDisplay;
    public CoinDropFetcher CoinDrop;

    private static readonly short MIN_DIFFICULTY = 0;
    private static readonly short MAX_DIFFICULTY = 10;
    [Space]
    [Header("Other")]
    [SerializeField] private float ButtonSpacingDuration = 0.05f;
    [SerializeField] private MMF_Player InteractSoundPlayer;

    [SerializeField]
    [MMReadOnly] private float _timeStamp = 0f;

    private short _lastLevel;
    private bool _firstTouch = false;


    public static float GetDifficultyModifier(int input_value, float final_multiplier)
    {
        int input = Mathf.Clamp(input_value, MIN_DIFFICULTY, MAX_DIFFICULTY);

        // Calculate the exponent base such that 10^0 = 1 and 10^1 = final_multiplier
        float baseValue = (float)Math.Log(final_multiplier) / 10.0f;

        float finalMultiplier = (float)Math.Exp(baseValue * input);

        // Calculate the result by raising e to the power of baseValue * input
        return finalMultiplier;
    }

    public void StartChangingDifficulty()
    {
        _timeStamp = Time.time;
        _firstTouch = true;
    }

    private void DifficultyChangeStep(bool addLevel)
    {
        _lastLevel = DifficultyLevel;
        if (addLevel)
        {
            DifficultyLevel++;
        }
        else
        {
            DifficultyLevel--;
        }

        
        DifficultyLevel = (short)Mathf.Clamp(DifficultyLevel, MIN_DIFFICULTY, MAX_DIFFICULTY);

        if (DifficultyLevel != _lastLevel)
        {
            DifficultyDisplay.FetchData(DifficultyLevel, DifficultyLevel / (float)MAX_DIFFICULTY);
            CoinDrop.FetchData(DifficultyLevel);
            InteractSoundPlayer.PlayFeedbacks();
        }
    }

    public void IncrementDifficulty()
    {
        if ((Time.time - _timeStamp) >= ButtonSpacingDuration || _firstTouch)
        {
            _timeStamp = Time.time;
            _firstTouch = false;

            DifficultyChangeStep(true);
        }
    }


    public void DecrementDifficulty()
    {
        if ((Time.time - _timeStamp) >= ButtonSpacingDuration || _firstTouch)
        {
            _timeStamp = Time.time;
            _firstTouch = false;

            DifficultyChangeStep(false);
        }
    }

    public void OneStepDifficulty(bool increment)
    {
        _lastLevel = DifficultyLevel;
        
        if (increment) DifficultyLevel++;
        else DifficultyLevel--;

        DifficultyLevel = (short)Mathf.Clamp(DifficultyLevel, MIN_DIFFICULTY, MAX_DIFFICULTY);

        if (DifficultyLevel != _lastLevel)
        {
            DifficultyDisplay.FetchData(DifficultyLevel, DifficultyLevel / (float)MAX_DIFFICULTY);
            CoinDrop.FetchData(DifficultyLevel);
            InteractSoundPlayer.PlayFeedbacks();
        }
    }


    public void ConfirmDifficulty()
    {
        // save the chosen difficulty level
        ProgressManager.TryGetInstance().AdjustDifficulty(DifficultyLevel);
        MMGameEvent.Trigger("TriggerSave");

        //starts the game if there's an existing session manager
        if (SessionManager.Current != null)
        {
            // starts the countdown and applies any player stat adjustments
            MMGameEvent.Trigger("EndSessionPrep");
            MMGameEvent.Trigger("ApplyPlayerStats");

            // turn off the panel
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No SessionManager present in scene to start!");
        }

        SessionManager.Current?.SetCoinReward(ProgressManager.TryGetInstance().GetGemReward());
    }

    private void Start()
    {
        // initialize difficulty level display
        // get the player's last difficulty level and display it as text
        LocalEdushooterStorage cache = ProgressManager.TryGetInstance().LocalCache;
        DifficultyLevel = cache.DifficultyLevel;

        //display it as text
        //DifficultyDisplay.FetchData(data, DifficultyLevel/MAX_DIFFICULTY);
        DifficultyDisplay.FetchData(DifficultyLevel, DifficultyLevel / (float)MAX_DIFFICULTY);
        CoinDrop.FetchData(cache.DifficultyLevel);
    }
}
