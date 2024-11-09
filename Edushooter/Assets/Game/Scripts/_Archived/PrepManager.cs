using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepManager : MonoBehaviour
{
    [System.Serializable]
    public enum PrepState
    {
        DifficultySelect,
        MapSelect,
    }

    public PrepState state = PrepState.DifficultySelect;

    [Space]
    public RectTransform DifficultyPanel;
    public RectTransform MapPanel;

    private void Start()
    {
        DifficultyPanel.gameObject.SetActive(true);
        MapPanel.gameObject.SetActive(false);
    }

    public void SwitchToMapSelect()
    {
        state = PrepState.MapSelect;

        DifficultyPanel.gameObject.SetActive(false);
        MapPanel.gameObject.SetActive(true);
        MMGameEvent.Trigger("SwitchedPanel");
    }

    public void SwitchToDifficultySelect()
    {
        state = PrepState.DifficultySelect;

        DifficultyPanel.gameObject.SetActive(true);
        MapPanel.gameObject.SetActive(false);
    }
}
