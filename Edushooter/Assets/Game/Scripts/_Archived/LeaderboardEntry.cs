using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EntryName;
    [SerializeField] private TextMeshProUGUI EntryPoints;
    [SerializeField] private TextMeshProUGUI EntryNumber;
    [SerializeField] private TextMeshProUGUI EntryDate;

    public static string SecondsToDateTimeString(int seconds)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(seconds).ToLocalTime();
        return dtDateTime.ToString("dd-MM-yyyy");
    }

    public void SetEntry(string name, int pts, int rank, int seconds)
    {
        EntryName.text = name;
        EntryPoints.text = pts.ToString();
        EntryNumber.text = rank.ToString();
        EntryDate.text = SecondsToDateTimeString(seconds);
    }
}
