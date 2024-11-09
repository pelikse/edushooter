using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleInit : MonoBehaviour
{
    public MMTimeManager MMTimeManager;

    // Start is called before the first frame update
    void Start()
    {
        MMTimeManager.SetTimeScaleTo(1f);
        Time.timeScale = 1f;
    }
}
