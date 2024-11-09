using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupCondition : ScriptableObject
{
    public abstract void StartCondition(GameObject Owner);
    public abstract void EndCondition();
    public abstract void RepeatableConditionEffect();

    public float MinDuration;
    public float MaxDuration;

    [Space]

    public bool ConditionHasRepeatableEffect = false;
    public float EffectFrequency = 0f;

    [Space]
    public Sprite ConditionSprite;
    public Color ConditionColor;

    [Space]
    [Header("Description")]
    public string PowerupName;
    [TextArea(5,3)]
    public string PowerupDescription;
}
