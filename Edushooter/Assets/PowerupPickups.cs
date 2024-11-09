using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class PowerupPickups : PickableItem
{
    public bool OnlyForPlayerCharacter = true;

    [Space]

    public PowerupCondition PowerUp;

    protected override void Pick(GameObject picker)
    {
        Character character = picker.gameObject.MMGetComponentNoAlloc<Character>();
        if (OnlyForPlayerCharacter && (character != null) && (_character.CharacterType != Character.CharacterTypes.Player))
        {
            return;
        }

        PlayerConditionManager conditionManager = picker.gameObject.MMGetComponentNoAlloc<PlayerConditionManager>();

        // else, we give the powerup to the player
        if (conditionManager != null)
        {
            Debug.Log("adding new powerup");
            conditionManager.AddCondition(PowerUp);
        }
        // also, remove the indicator
        PowerupIndicatorManager.TryGetInstance()?.RemovePowerup(gameObject);
    }

    public void ActivateModel()
    {
        if (!Model.activeInHierarchy)
        {
            Model.SetActive(true);
        }

        if (_collider != null)
        {
            _collider.enabled = true;
        }
        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }

        gameObject.SetActive(true );
    }
}
