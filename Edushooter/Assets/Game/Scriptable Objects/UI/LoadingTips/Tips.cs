using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tip", menuName = "Tips Info")]
public class Tips : ScriptableObject
{
    [TextArea(3,3)]
    public string tipDescription;
}
