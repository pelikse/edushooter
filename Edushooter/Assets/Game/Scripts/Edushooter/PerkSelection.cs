using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PerkSelection : MonoBehaviour
{
    public PerkScriptable PerkData;

    [Space]

    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private Image Icon;

    [Space]

    [SerializeField] private ModifiableButtons PerkButton;

    public void _Initialize(PerkScriptable perk, Sprite RaritySprite)
    {
        PerkData = perk;

        Title.text = perk.PerkName;
        Desc.text = perk.PerkDescription;

        if (perk.PerkIcon != null)
        {
            Icon.sprite = perk.PerkIcon;
        }

        PerkButton.ChangeInitialSprite(RaritySprite);
    }

    public void SetPerkSelection()
    {
        // apply the perk effect then end upgrade
        PerkData.ApplyPerkEffect();

        //end upgrade phase
        MMGameEvent.Trigger("UpgradeEnd");
    }
}
