using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;

public class TextCycler : MonoBehaviour
{
    [MMInformation("This script cycles a TextMeshProUGUI component between several text with an adjustable interval.", MMInformationAttribute.InformationType.Info, true)]
    [MMReadOnly]
    public bool info;

    [SerializeField] protected TextMeshProUGUI TextMesh;

    [Space,Space]

    [TextArea(2,2)]
    [SerializeField] protected string[] TextToCycle;
    [SerializeField] protected float CycleInterval;


    private bool _startCycle = false;

    private int i = 0;
    private int _cycleLength = 0;

    private float _lastTime;

    // Start is called before the first frame update
    void Start()
    {
        if (TextToCycle != null && TextToCycle.Length > 0)
        {
            _startCycle = true;

            _cycleLength = TextToCycle.Length;
            _lastTime = Time.time;
        }
        else
        {
            _startCycle = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_startCycle)
        {
            // if we've exceeded the interval, then cycle and start a new interval
            if (Time.time - _lastTime > CycleInterval)
            {
                // get the next index (looping)
                i = (i+1) % _cycleLength;

                TextMesh.text = TextToCycle[i];
                _lastTime = Time.time;
            }
        }
    }
}
