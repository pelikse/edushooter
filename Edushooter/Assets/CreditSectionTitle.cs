using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditSectionTitle : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;

    public void Init(string title)
    {
        TextMesh.text = title;
    }
}
