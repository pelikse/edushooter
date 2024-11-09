using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DamageOnTouchExtended : DamageOnTouch
{
    [MMInspectorGroup("Extensions", true, 20)]
    /// the feedback to play when hitting a Damageable
    [Tooltip("remove all vertical (Y-axis) knockback from the damage")]
    public bool RemoveVerticalKnockback = true;

    protected override void ApplyKnockback(float damage, List<TypedDamage> typedDamages)
    {
        if (ShouldApplyKnockback(damage, typedDamages))
        {
            _knockbackForce = DamageCausedKnockbackForce * _colliderHealth.KnockbackForceMultiplier;
            _knockbackForce = _colliderHealth.ComputeKnockbackForce(_knockbackForce, typedDamages);

            if (_twoD) // if we're in 2D
            {
                ApplyKnockback2D();
            }
            else // if we're in 3D
            {
                ApplyKnockback3D();
            }

            if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
            {
                // remove all Y-axis force from the knockback
                if (RemoveVerticalKnockback)
                {
                    _knockbackForce.y = 0f;
                }

                _colliderTopDownController.Impact(_knockbackForce.normalized, _knockbackForce.magnitude);
            }
        }
    }
}
