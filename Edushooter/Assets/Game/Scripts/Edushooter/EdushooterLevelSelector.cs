using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdushooterLevelSelector : MonoBehaviour
{
    public LevelSelector LevelSelector;

    public void GoToSelectedLevel()
    {
        string selectedLevel = ProgressManager.TryGetInstance().LocalCache.SelectedMap.ToString();
        Debug.Log(selectedLevel);

        if (selectedLevel == "")
        {
            selectedLevel = "Neighborhood";
        }

        LevelSelector.LevelName = selectedLevel + " Scene";

        MMGameEvent.Trigger("UnpauseSession");

        LevelSelector.GoToLevel();
    }

    public void GoToMainMenu()
    {
        LevelSelector.LevelName = "Level Selection";

        MMGameEvent.Trigger("UnpauseSession");

        LevelSelector.GoToLevel();
    }

    public void GoToLevelSelectorLevel()
    {
        MMGameEvent.Trigger("UnpauseSession");

        LevelSelector.GoToLevel();
    }
}
