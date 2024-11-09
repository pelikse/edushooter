using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomObjects", menuName = "Custom/QuestionInfo")]
public class QuestionInfo : ScriptableObject
{
    [TextArea(3,3)]
    public string QuestionDescription;
}
