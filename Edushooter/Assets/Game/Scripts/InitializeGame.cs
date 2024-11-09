using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    public GameObject LoadingUI;
    public GameObject StartButtons;

    private void Start()
    {
        LoadingUI.SetActive(true);
        StartButtons.SetActive(false);
    }

    public void InitGame()
    {
        LoadingUI.SetActive(false);
        StartButtons.SetActive(true);
    }
}
