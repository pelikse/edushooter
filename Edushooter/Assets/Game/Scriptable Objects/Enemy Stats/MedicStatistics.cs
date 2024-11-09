using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomObjects", menuName = "MedicStats")]
public class MedicStatistics : ScriptableObject
{
    public float BaseHealing = 5f;
    public float BaseResistance = 0.2f;
}
