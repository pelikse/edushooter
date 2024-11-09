using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameInfoFetcher : MonoBehaviour
{

    [Header("Display Objects")]
    [SerializeField] private VideoPlayer DemoPlayer;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;

    public void FetchEdugymInfo(EdugymDescription desc)
    {
        DemoPlayer.clip = desc.GameplayDemo;

        Title.text = desc.Title;
        Description.text = desc.Desc;
    }


}
