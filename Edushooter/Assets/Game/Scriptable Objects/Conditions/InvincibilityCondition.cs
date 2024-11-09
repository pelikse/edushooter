using HighlightPlus;
using MoreMountains.TopDownEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Invincibility")]
public class InvincibilityCondition : PowerupCondition
{
    private Health ownerHealth;
    private HighlightEffect playerHighlight;

    private Color defaultOutlineColor;
    private float defaultOutlineWidth;

    public override void EndCondition()
    {
        if (ownerHealth != null)
        {
            Debug.Log("ending invincibility...");
            ownerHealth.DamageEnabled();
        }

        if (playerHighlight != null)
        {
            playerHighlight.outlineColor = defaultOutlineColor;
            playerHighlight.outlineWidth = defaultOutlineWidth;
        }
    }

    public override void StartCondition(GameObject Owner)
    {
        ownerHealth = Owner.GetComponent<Health>();
        playerHighlight = Owner.GetComponentInChildren<HighlightEffect>();

        if (ownerHealth != null)
        {
            Debug.Log("starting invincibility...");
            ownerHealth.DamageDisabled();
        }
        else
        {
            Debug.LogError("owner has no health class!");
        }

        if (playerHighlight != null)
        {
            defaultOutlineColor = playerHighlight.outlineColor;
            defaultOutlineWidth = playerHighlight.outlineWidth;

            //set new outline
            playerHighlight.outlineColor = Color.white;
            playerHighlight.outlineWidth = 0.4f;
        }
    }

    public override void RepeatableConditionEffect()
    {
        throw new NotImplementedException();    
    }
}
