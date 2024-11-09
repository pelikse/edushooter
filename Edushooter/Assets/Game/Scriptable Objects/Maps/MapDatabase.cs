using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDatabase", menuName = "Map Database")]
public class MapDatabase : ScriptableObject
{
    public MapInfo[] maps;

    public int[] GetTwoDistinctMapIndex()
    {
        var uniqueElements = maps.Distinct().ToList();
        
        try
        {
            if (uniqueElements.Count < 2)
            {
                throw new ArgumentException("Input array must contain at least two unique elements.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return new int[2] { 0, 1 };
        }

        int indexA = UnityEngine.Random.Range(0, uniqueElements.Count);
        int indexB;
        do
        {
            indexB = UnityEngine.Random.Range(0, uniqueElements.Count);
        } while (indexA == indexB);

        return new int[2] { indexA, indexB };
    }

    public MapInfo[] GetTwoDistinctMapTypes()
    {
        var uniqueElements = maps.Distinct().ToList();

        try
        {
            if (uniqueElements.Count < 2)
            {
                throw new ArgumentException("Input array must contain at least two unique elements.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return new MapInfo[2] { maps[0], maps[1] };
        }

        int indexA = UnityEngine.Random.Range(0, uniqueElements.Count);
        int indexB;
        do
        {
            indexB = UnityEngine.Random.Range(0, uniqueElements.Count);
        } while (indexA == indexB);

        return new MapInfo[2] { maps[indexA], maps[indexB] };
    }

    public MapInfo GetMapFromIndex(int index)
    {
        return maps[index];
    }

    public MapInfo GetMapFromType(MapType map)
    {
        foreach (MapInfo entry in maps)
        {
            if (entry.mapType == map)
            {
                return entry;
            }
        }

        Debug.LogWarning("didnt find the requested map, check the map database for errors!");
        return maps[0];
    }
}
