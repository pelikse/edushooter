using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryOverlay : MonoBehaviour
{
    public CoinDropFetcher GemDisplay;

    private void OnEnable()
    {
        GemDisplay.PutCoinValue(SessionManager.Current.CoinReward);
    }
}
