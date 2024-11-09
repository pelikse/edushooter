using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/Trailblazer")]
public class TrailblazerCondition : PowerupCondition
{
    [Space, Space]
    [Header("Trailblazer Attributes")]
    public float MovementSpeedMultiplier;
    public short ExtraDashCharges;

    private CharacterAbilityTeleport ownerTeleport;
    private CharacterMovement ownerMovement;

    private int _normalDashCharges;
    private float _normalSpeed;

    public override void StartCondition(GameObject Owner)
    {
        if (Owner.TryGetComponent(out ownerTeleport))
        {
            _normalDashCharges = ownerTeleport.MaxCharges;
            ownerTeleport.SetMaxCharges(_normalDashCharges+ExtraDashCharges);
        }

        if (Owner.TryGetComponent(out ownerMovement))
        {
            _normalSpeed = ownerMovement.WalkSpeed;
            ownerMovement.WalkSpeed = _normalSpeed * MovementSpeedMultiplier;
            ownerMovement.ResetSpeed();
        }
    }

    public override void EndCondition()
    {
        if (ownerTeleport != null)
        {
            ownerTeleport.SetMaxCharges(_normalDashCharges);
        }

        if (ownerMovement != null)
        {
            ownerMovement.WalkSpeed = _normalSpeed;
            ownerMovement.ResetSpeed();
        }
    }

    public override void RepeatableConditionEffect()
    {

    }
}
