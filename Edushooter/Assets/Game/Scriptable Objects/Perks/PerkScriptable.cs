using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PerkScriptable : ScriptableObject
{
    [System.Serializable]
    public enum PerkRarity
    {
        Common,
        Exotic,
        Legendary
    }

    public string PerkName;

    [TextArea(5,8)]
    public string PerkDescription;

    public Sprite PerkIcon;

    [Space]

    public PerkRarity Rarity;
    public bool SelectOnce = false;

    public abstract void ApplyPerkEffect();
}
