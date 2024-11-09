using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinDropFetcher : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    public void FetchData(int difficulty)
    {
        tmp.text = ProgressManager.CalculateGemReward(difficulty).ToString();
    }

    public void PutCoinValue(int coin)
    {
        tmp.text = coin.ToString();
    }
}
