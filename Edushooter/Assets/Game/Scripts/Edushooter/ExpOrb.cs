using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class ExpOrb : PickableItem
{
    [Space]
    [Space]
    [Header("ExpOrb")]
    // exp gained when collected
    public float ExpToGive = 2f;

    [Space]
    [Header("Objects To Enable")]
    public GameObject ModelToEnable;
    public MMBlink BlinkingScript;
    public Collider ColliderToEnable;

    public void TurnOff()
    {
        ModelToEnable.SetActive(false);
        BlinkingScript.enabled = false;
        ColliderToEnable.enabled = false;
    }

    public void TurnOn()
    {
        ModelToEnable.SetActive(true);
        BlinkingScript.enabled = true;
        ColliderToEnable.enabled = true;
    }

    public void SetExpOrbValue(float exp)
    {
        ExpToGive = exp;
    }

    protected override void Pick(GameObject picker)
    {

        Character character = picker.gameObject.MMGetComponentNoAlloc<Character>();
        if ((character != null) && (_character.CharacterType != Character.CharacterTypes.Player))
        {
            return;
        }

        // give the character exp
        Levels characterLevel = picker.gameObject.MMGetComponentNoAlloc<Levels>();

        if (characterLevel != null)
        {
            characterLevel.ReceiveExp(ExpToGive);
        }
        // turn off model and blinking
        TurnOff();
    }
}
