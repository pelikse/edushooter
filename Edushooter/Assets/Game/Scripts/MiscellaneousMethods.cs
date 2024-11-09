using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MiscellaneousMethods
{
    //this class holds all of the miscellaneous methods that can be used globally
    //it is not designed to be used in game
    //all methods are public and static

    public static string TruncateString(string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str) || maxLength < 0)
        {
            return string.Empty;
        }

        return str.Length <= maxLength ? str : str[..maxLength];
    }
    public static void PrintDictionaryContents<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
        {
            Debug.LogFormat($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }
    public static int ClosestLowerMultipleOfFive(int number)
    {
        return number - (number % 5);
    }
    public static string ConvertTimestampToDateString(Timestamp timestamp)
    {
        // Convert Firebase Timestamp to DateTime
        DateTime dateTime = timestamp.ToDateTime();

        // Format the DateTime to dd-MM-yyyy
        string formattedDate = dateTime.ToString("dd-MM-yyyy");

        return formattedDate;
    }

    public static double TimeSinceTimestamp(Timestamp referenceTimestamp)
    {
        // Convert the reference timestamp to DateTime
        DateTime referenceDateTime = referenceTimestamp.ToDateTime();

        // Get the current timestamp and convert it to DateTime
        DateTime currentDateTime = Timestamp.GetCurrentTimestamp().ToDateTime();

        // Calculate the difference between the current time and reference time
        TimeSpan timeDifference = currentDateTime - referenceDateTime;

        // Check if the difference is no more than 24 hours (1 day)
        return timeDifference.TotalHours;
    }
}
