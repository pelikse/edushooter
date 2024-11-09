using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyLevelFetcher : MonoBehaviour
{
    public TextMeshProUGUI DifficultyText;

    [Space]
    [Header("Text Color Range")]
    public float startNumber = 1f;
    public float endNumber = 0.2f;

    public float MapFractionToTextColor(float fraction)
    {
        return (startNumber + (endNumber - startNumber) * fraction);
    }

    public void FetchData(LocalEdushooterStorage data)
    {
        DifficultyText.text = data.DifficultyLevel.ToString();
    }

    public void FetchData(LocalEdushooterStorage data, float textColor)
    {
        DifficultyText.text = data.DifficultyLevel.ToString();

        float newColor = MapFractionToTextColor(textColor);
        DifficultyText.color = new Color(1f, newColor, newColor);
    }

    public void FetchData(short _difficultyValue, float textColor)
    {
        DifficultyText.text = _difficultyValue.ToString();

        float newColor = MapFractionToTextColor(textColor);
        DifficultyText.color = new Color(1f, newColor, newColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        DifficultyText.color = Color.white;
        FetchData(ProgressManager.TryGetInstance().LocalCache);
    }
}
