using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBulletDebug : MonoBehaviour
{
    public void DebugRPG(GameObject go)
    {
        Debug.Log(go.gameObject + " is hit");
    }

    public void DebugRPG(Health go)
    {
        Debug.Log(go.gameObject + " enemy is hit");
    }
}
