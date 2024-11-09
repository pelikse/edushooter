using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShufflePerks : MonoBehaviour
{

    public void ShufflePerkSelection()
    {
        MMGameEvent.Trigger("ShufflePerks");
    }
}
