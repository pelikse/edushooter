using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreditSection", menuName = "Custom/Credit Section")]
public class CreditSection : ScriptableObject
{
    [System.Serializable]
    public class CreditRow
    {
        public string Role;
        public string Name;
    }

    public string SectionTitle;

    public List<CreditRow> Attributions;
}
