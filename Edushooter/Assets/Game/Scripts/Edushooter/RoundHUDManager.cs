using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;

public class RoundHUDManager : MMSingleton<RoundHUDManager>, MMEventListener<MMGameEvent>
{
    [SerializeField] private TextMeshProUGUI HUD_RoundsPassed;
    [SerializeField] private TextMeshProUGUI HUD_EnemiesLeft;

    [Space]
    [Header("Text Formatting")]

    // how should the rounds and enemies left be displayed, format in C# to allow for 1 parameter
    public string RoundsPassedFormat = "Round {0}";
    public string EnemiesLeftFormat = "{0} Enemies Left";
    public string IntermissionFormat = "Round Intermission...";

    public void SetRoundsPassed(int roundsPassed)
    {
        HUD_RoundsPassed.text = string.Format(RoundsPassedFormat, roundsPassed);
    }

    public void SetEnemiesLeft(int enemiesLeft)
    {
        HUD_EnemiesLeft.text = string.Format(EnemiesLeftFormat, enemiesLeft);
    }

    // implement event listener
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
        if (eventType.EventName.Equals("IntermissionStart"))
        {
            HUD_EnemiesLeft.text = IntermissionFormat;
        }
    }
}
