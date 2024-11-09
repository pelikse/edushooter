using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipGenerator : MonoBehaviour
{
    public Tips[] tips;
    public TextMeshProUGUI tmp;
    private void OnEnable()
    {
        string desc;

        try
        {
            desc = tips[Random.Range(0, tips.Length)].tipDescription;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            desc = "";
        }

        tmp.text = desc;
    }
}
