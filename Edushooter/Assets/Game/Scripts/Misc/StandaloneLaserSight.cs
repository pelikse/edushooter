using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneLaserSight : WeaponLaserSight
{
    [MMInformation("This script creates a laser sight by inheriting from the WeaponLaserSight script and removing everything related to weapons. It emits laser from the transform that this script is attached to.", MMInformationAttribute.InformationType.Info, true)]
    [MMReadOnly]
    public bool Info;

    public override void ShootLaser()
    {
        if (!PerformRaycast)
        {
            return;
        }

        _laserOffset = LaserOriginOffset;

        _weaponPosition = transform.position;
        _weaponRotation = transform.rotation;

        _thisPosition = this.transform.position;
        _thisRotation = this.transform.rotation;
        _thisForward = this.transform.forward;

        if (Mode == Modes.ThreeD)
        {
            // our laser will be shot from the weapon's laser origin
            _origin = MMMaths.RotatePointAroundPivot(_thisPosition + _laserOffset, _thisPosition, _thisRotation);
            _raycastOrigin = MMMaths.RotatePointAroundPivot(_thisPosition + RaycastOriginOffset, _thisPosition, _thisRotation);

            // we cast a ray in front of the weapon to detect an obstacle
            _hit = MMDebug.Raycast3D(_raycastOrigin, _thisForward, LaserMaxDistance, LaserCollisionMask, Color.red, true);

            // if we've hit something, our destination is the raycast hit
            if (_hit.transform != null)
            {
                _destination = _hit.point;
            }
            // otherwise we just draw our laser in front of our weapon 
            else
            {
                _destination = _origin + _thisForward * LaserMaxDistance;
            }
        }
        else
        {
            _direction = _weapon.Flipped ? Vector3.left : Vector3.right;
            if (_direction == Vector3.left)
            {
                _laserOffset.x = -LaserOriginOffset.x;
            }

            _raycastOrigin = MMMaths.RotatePointAroundPivot(_weaponPosition + _laserOffset, _weaponPosition, _weaponRotation);
            _origin = _raycastOrigin;

            // we cast a ray in front of the weapon to detect an obstacle
            _hit2D = MMDebug.RayCast(_raycastOrigin, _weaponRotation * _direction, LaserMaxDistance, LaserCollisionMask, Color.red, true);
            if (_hit2D)
            {
                _destination = _hit2D.point;
            }
            // otherwise we just draw our laser in front of our weapon 
            else
            {
                _destination = _origin;
                _destination.x = _destination.x + LaserMaxDistance * _direction.x;
                _destination = MMMaths.RotatePointAroundPivot(_destination, _weaponPosition, _weaponRotation);
            }
        }

        if (Time.frameCount <= _initFrame + 1)
        {
            return;
        }

        // we set our laser's line's start and end coordinates
        if (DrawLaser)
        {
            _line.SetPosition(0, _origin);
            _line.SetPosition(1, _destination);
        }
    }
}
