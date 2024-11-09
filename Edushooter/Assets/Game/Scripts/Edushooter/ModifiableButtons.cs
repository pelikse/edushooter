using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableButtons : MMTouchButton
{
    public void ChangeInitialSprite(Sprite newSprite)
    {
        _initialSprite = newSprite;
    }
}
