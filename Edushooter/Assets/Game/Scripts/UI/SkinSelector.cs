using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    public enum SkinType
    {
        Head,
        Body
    }

    public SkinType skinType = SkinType.Head;
    public ModelType modelType = ModelType.Guerilla;
}
