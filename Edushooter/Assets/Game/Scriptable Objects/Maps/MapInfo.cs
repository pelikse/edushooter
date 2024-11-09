using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "MapInfo")]
[System.Serializable]
public class MapInfo : ScriptableObject
{
    public MapType mapType;
    public string mapName;

    [TextArea(3,5)]
    public string mapDesc;

    [Space]

    public Sprite mapSprite;
}
