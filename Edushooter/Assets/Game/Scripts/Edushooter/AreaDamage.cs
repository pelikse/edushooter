using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static MoreMountains.TopDownEngine.DamageOnTouch;

public class AreaDamage : MMMonoBehaviour
{
    [MMInspectorGroup("Targets", true, 5)]
    // what layermask do our targets
    public LayerMask TargetLayerMask;

    [Tooltip("the owner of the DamageOnTouch zone")]
    public GameObject Owner;

    [MMInspectorGroup("Area of Effect", true, 4)]
    public float AreaRadius = 5f;
    
    public Transform AreaOrigin;

    public bool CheckLineOfSight = true;

    public LayerMask ObstructionLayers;

    [MMInspectorGroup("Damage Caused", true, 8)]
    /// The min amount of health to remove
    [FormerlySerializedAs("DamageCaused")]
    [Tooltip("The min amount of health to remove from the player's health")]
    public float MinDamageCaused = 10f;

    /// The max amount of health to remove from the player's health
    [Tooltip("The max amount of health to remove from the player's health")]
    public float MaxDamageCaused = 10f;

    /// a list of typed damage definitions that will be applied on top of the base damage
    [Tooltip("a list of typed damage definitions that will be applied on top of the base damage")]
    public List<TypedDamage> TypedDamages;

    /// should the area damage ignore the owner?
    [Tooltip("owner will be ignored in the damage calculations")]
    public bool IgnoreOwner = true;

    [Header("Knockback")]
    /// the type of knockback to apply when causing damage
    [Tooltip("the type of knockback to apply when causing damage")]
    public KnockbackStyles DamageCausedKnockbackType = KnockbackStyles.AddForce;

    /// The direction to apply the knockback 
    [Tooltip("The direction to apply the knockback ")]
    [MMReadOnly]
    public KnockbackDirections DamageCausedKnockbackDirection = KnockbackDirections.BasedOnOwnerPosition;

    /// The force to apply to the object that gets damaged
    [Tooltip("The force to apply to the object that gets damaged")]
    public Vector3 DamageCausedKnockbackForce = new Vector3(10, 10, 10);

    [Tooltip("The force to apply to the object that gets damaged")]
    public bool RemoveVerticalKnockback = true;

    [Header("Invincibility")]
    /// The duration of the invincibility frames after the hit (in seconds)
    [Tooltip("The duration of the invincibility frames after the hit (in seconds)")]
    public float InvincibilityDuration = 0.5f;


    [MMInspectorGroup("Feedbacks", true, 18)]
    /// the feedback to play when hitting anything
    [Tooltip("the feedback to play when damage is activated")]
    public MMFeedbacks OnActivationFeedback;


    //storage
    private Collider[] _hitColliders;
    private int _hitCollidersCount;
    private float _randomDamage;
    private Health _colliderHealth;
    private Vector3 _areaOrigin, _damageDirection, _knockbackForce, _relativePosition, _targetDirection;
    private TopDownController _colliderTopDownController;


    private void Start()
    {
        Owner = gameObject;
    }

    public virtual void TriggerAreaDamage()
    {
        // determine the origin of our damage
        DetermineDamageOrigin();

        // get the relevant colliders
        _hitColliders = Physics.OverlapSphere(_areaOrigin, AreaRadius, TargetLayerMask);
        _hitCollidersCount = _hitColliders.Length;

        // if there's at least 1 collider to deal damage to
        if (_hitCollidersCount > 0 )
        {
            foreach (Collider collider in _hitColliders)
            {
                // check if we should apply damage to this collider
                if (!EvaluateAvailability(collider.gameObject)) continue;

                // try and get the health components
                _colliderHealth = collider.gameObject.MMGetComponentNoAlloc<Health>();

                if ( _colliderHealth != null)
                {
                    if (_colliderHealth.CanTakeDamageThisFrame())
                    {
                        // if what we're colliding with is a TopDownController, we apply a knockback force
                        _colliderTopDownController = _colliderHealth.gameObject.MMGetComponentNoAlloc<TopDownController>();

                        // play the feedback
                        OnActivationFeedback?.PlayFeedbacks(transform.position);

                        // get random damage
                        _randomDamage = Random.Range(MinDamageCaused, MaxDamageCaused);

                        // apply knockback
                        ApplyKnockback(_randomDamage, TypedDamages);

                        // determine the damage direction
                        DetermineDamageDirection();

                        _colliderHealth.Damage(_randomDamage, gameObject, InvincibilityDuration, InvincibilityDuration,
                            _damageDirection, TypedDamages);
                    }
                }
            }
        }
    }

    protected virtual bool EvaluateAvailability(GameObject colliderChecked)
    {
        // if we're inactive, we do nothing
        if (!isActiveAndEnabled) { return false; }

        // if the object we're colliding with is part of our ignore list, we do nothing and exit
        if (IgnoreOwner)
        {
            if (colliderChecked.Equals(Owner)) { return false; }
        }

        // if we're on our first frame, we don't apply damage
        if (Time.time == 0f) { return false; }

        // should we check for line of sight
        if (CheckLineOfSight)
        {
            // if we dont have LOS, then skip this target
            return CheckLOS(colliderChecked.transform);
        }

        return true;
    }

    protected virtual bool CheckLOS(Transform target)
    {
        _targetDirection = target.position - _areaOrigin;
        
        if (!Physics.Raycast(_areaOrigin, _targetDirection, AreaRadius, ObstructionLayers))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void DetermineDamageOrigin()
    {
        if (AreaOrigin != null)
        {
            _areaOrigin = AreaOrigin.transform.position;
        }
        else
        {
            _areaOrigin = Owner.transform.position;
        }
    }

    protected virtual void DetermineDamageDirection()
    {
        _damageDirection = _colliderHealth.transform.position - _areaOrigin;
        _damageDirection = _damageDirection.normalized;
    }

    protected virtual void ApplyKnockback(float damage, List<TypedDamage> typedDamages)
    {
        // if we should apply knockback
        if (ShouldApplyKnockback(damage, typedDamages))
        {
            // calculate the knockback force
            _knockbackForce = DamageCausedKnockbackForce * _colliderHealth.KnockbackForceMultiplier;
            _knockbackForce = _colliderHealth.ComputeKnockbackForce(_knockbackForce, typedDamages);

            ApplyKnockback3D();

            if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
            {
                // if we should remove the vertical knockback (Y axis)
                if (RemoveVerticalKnockback)
                {
                    _knockbackForce.y = 0;
                }

                _colliderTopDownController.Impact(_knockbackForce.normalized, _knockbackForce.magnitude);
            }
        }
    }

    protected virtual void ApplyKnockback3D()
    {
        _relativePosition = _colliderTopDownController.transform.position - _areaOrigin;
        _knockbackForce = Quaternion.LookRotation(_relativePosition) * _knockbackForce;

        #region OtherKnockbackTypes
        //switch (DamageCausedKnockbackDirection)
        //{
        //    case KnockbackDirections.BasedOnSpeed:
        //        var totalVelocity = _colliderTopDownController.Speed + _velocity;
        //        _knockbackForce = _knockbackForce * totalVelocity.magnitude;
        //        break;
        //    case KnockbackDirections.BasedOnOwnerPosition:

        //        break;
        //    case KnockbackDirections.BasedOnDirection:
        //        var direction = transform.position - _positionLastFrame;
        //        _knockbackForce = direction * _knockbackForce.magnitude;
        //        break;
        //    case KnockbackDirections.BasedOnScriptDirection:
        //        _knockbackForce = _knockbackScriptDirection * _knockbackForce.magnitude;
        //        break;
        //}
        #endregion
    }

    protected virtual bool ShouldApplyKnockback(float damage, List<TypedDamage> typedDamages)
    {
        if (_colliderHealth.ImmuneToKnockbackIfZeroDamage)
        {
            if (_colliderHealth.ComputeDamageOutput(damage, typedDamages, false) == 0)
            {
                return false;
            }
        }

        return (_colliderTopDownController != null)
               && (DamageCausedKnockbackForce != Vector3.zero)
               && !_colliderHealth.Invulnerable
               && _colliderHealth.CanGetKnockback(typedDamages);
    }
}
