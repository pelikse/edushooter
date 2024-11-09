using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionCrowdPathfind3D : AIActionPathfinderToTarget3D
{
    //overrides
    protected CrowdPathfinder3D _crowdPathfinder3D;

    //extensions
    [Space, Space]

    [MMInformation("This AI Action uses the CrowdPathfinder3D instead of AIActionPathfinderToTarget3D. Make sure you put the correct script!", MMInformationAttribute.InformationType.Warning, true)]
    public float MaxDistanceFromTarget = 4f;

    [Space]

    public bool RandomizeTargetPosition = true;

    public override void Initialization()
    {
        if (!ShouldInitialize) return;
        base.Initialization();
        _characterMovement = gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
        _crowdPathfinder3D = gameObject.GetComponentInParent<Character>()?.FindAbility<CrowdPathfinder3D>();
        if (_crowdPathfinder3D == null)
        {
            Debug.LogWarning(name + " : the AIActionPathfinderToTarget3D AI Action requires the CrowdPathfinder3D ability");
        }
    }

    protected override void Move()
    {
        if (Time.time - _lastSetNewDestinationAt < MinimumDelayBeforeUpdatingTarget)
        {
            return;
        }

        _lastSetNewDestinationAt = Time.time;

        if (_brain.Target == null)
        {
            _crowdPathfinder3D.SetNewDestination(null);
            return;
        }
        else
        {
            //if we randomize then we use the overload
            if (RandomizeTargetPosition) _crowdPathfinder3D.SetNewDestination(_brain.Target.transform, MaxDistanceFromTarget);
            //else then not
            else _crowdPathfinder3D.SetNewDestination(_brain.Target.transform);
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();

        _crowdPathfinder3D?.SetNewDestination(null);
        _characterMovement?.SetHorizontalMovement(0f);
        _characterMovement?.SetVerticalMovement(0f);
    }
}
