using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditSectionRow : MonoBehaviour
{
    public TextMeshProUGUI TextMeshCategory;
    public TextMeshProUGUI TextMeshDescription;

    public void Init(string category, string desc)
    {
        TextMeshCategory.text = category;
        TextMeshDescription.text = desc;
    }
}
