using UnityEngine;
using MoreMountains.TopDownEngine;
using System;
using UnityEngine.AI;
using Unity.VisualScripting;

public class CrowdPathfinder3D : CharacterPathfinder3D
{
    public float MinimumOffset = 0.2f;
    public float MaximumOffset = 0.5f;

    //cache
    private float _lastPathDistance = 0f;
    private float _pathFoundDistance = 0f;

    private bool _travellingToNextWaypoint = false;
    //private bool _offsetCalculated = false;

    private Vector3 CalculatePositionWithinMaxDistance(Vector3 origin, float maxDistance)
    {
        // Generate random x and z offsets within the specified range
        float offsetX = UnityEngine.Random.Range(-maxDistance, maxDistance);
        float offsetZ = UnityEngine.Random.Range(-maxDistance, maxDistance);

        return origin + new Vector3(offsetX, 0f, offsetZ); ;
    }

    public virtual void SetNewDestination(Transform destinationTransform, float distance)
    {
        if (destinationTransform == null)
        {
            Target = null;
            return;
        }
        Target = destinationTransform;
        DeterminePathRandom(this.transform.position, CalculatePositionWithinMaxDistance(Target.position, distance));
    }
   

    protected void DeterminePathRandom(Vector3 startingPosition, Vector3 targetPosition, bool ignoreDelay = false)
    {
        if (!ignoreDelay && (Time.time - _lastRequestAt < MinimumDelayBeforePollingNavmesh))
        {
            return;
        }

        _lastRequestAt = Time.time;

        NextWaypointIndex = 0;

        // we find the closest position to the starting position on the navmesh
        _closestStartNavmeshPosition = startingPosition;
        if (NavMesh.SamplePosition(startingPosition, out _navMeshHit, ClosestPointThreshold, NavMesh.AllAreas))
        {
            _closestStartNavmeshPosition = _navMeshHit.position;
        }

        // we find the closest position to the target position on the navmesh
        _closestTargetNavmeshPosition = targetPosition;
        if (NavMesh.SamplePosition(targetPosition, out _navMeshHit, ClosestPointThreshold, NavMesh.AllAreas))
        {
            _closestTargetNavmeshPosition = _navMeshHit.position;
        }
        
        //check for a path to the new destination and also check the new path distance
        _pathFound = NavMesh.CalculatePath(_closestStartNavmeshPosition, _closestTargetNavmeshPosition, NavMesh.AllAreas, AgentPath);
        _pathFoundDistance = GetLengthOfPath(AgentPath);

        if (!_travellingToNextWaypoint)
        {
            _lastValidTargetPosition = _closestTargetNavmeshPosition;
            _lastPathDistance = _pathFoundDistance;
        }

        if (_pathFound && _pathFoundDistance > _lastPathDistance)
        {
            Debug.Log("currently travelling or the new path is longer than old path!");

            _lastValidTargetPosition = _closestTargetNavmeshPosition;
            _lastPathDistance = _pathFoundDistance;
        }
        else
        {
            NavMesh.CalculatePath(startingPosition, _lastValidTargetPosition, NavMesh.AllAreas, AgentPath);
        }

        // Waypoints = AgentPath.corners;
        _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
        if (_waypoints >= Waypoints.Length)
        {
            Array.Resize(ref Waypoints, _waypoints + 5);
            _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
        }
        if (_waypoints >= 2)
        {
            NextWaypointIndex = 1;
        }

        InvokeOnPathProgress(NextWaypointIndex, Waypoints.Length, Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]));
    }

    //protected virtual void DeterminePathOffset(Vector3 startingPosition, Vector3 targetPosition, bool ignoreDelay = false)
    //{
    //    if (!ignoreDelay && (Time.time - _lastRequestAt < MinimumDelayBeforePollingNavmesh))
    //    {
    //        return;
    //    }

    //    _lastRequestAt = Time.time;

    //    NextWaypointIndex = 0;

    //    // we find the closest position to the starting position on the navmesh
    //    _closestStartNavmeshPosition = startingPosition;
    //    if (NavMesh.SamplePosition(startingPosition, out _navMeshHit, ClosestPointThreshold, NavMesh.AllAreas))
    //    {
    //        _closestStartNavmeshPosition = _navMeshHit.position;
    //    }

    //    // we find the closest position to the target position on the navmesh
    //    _closestTargetNavmeshPosition = targetPosition;
    //    if (NavMesh.SamplePosition(targetPosition, out _navMeshHit, ClosestPointThreshold, NavMesh.AllAreas))
    //    {
    //        _closestTargetNavmeshPosition = _navMeshHit.position;
    //    }

    //    _pathFound = NavMesh.CalculatePath(_closestStartNavmeshPosition, _closestTargetNavmeshPosition, NavMesh.AllAreas, AgentPath);
    //    if (_pathFound)
    //    {
    //        _lastValidTargetPosition = _closestTargetNavmeshPosition;
    //    }
    //    else
    //    {
    //        NavMesh.CalculatePath(startingPosition, _lastValidTargetPosition, NavMesh.AllAreas, AgentPath);
    //    }

    //    // Waypoints = AgentPath.corners;
    //    _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
    //    if (_waypoints >= Waypoints.Length)
    //    {
    //        Array.Resize(ref Waypoints, _waypoints + 5);
    //        _waypoints = AgentPath.GetCornersNonAlloc(Waypoints);
    //    }
    //    if (_waypoints >= 2)
    //    {
    //        NextWaypointIndex = 1;
    //    }

    //    InvokeOnPathProgress(NextWaypointIndex, Waypoints.Length, Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]));
    //}


    private float GetLengthOfPath(NavMeshPath path)
    {
        float distance = 0f;

        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i-1], path.corners[i]);
        }

        return distance;
    }

    protected override void DetermineNextWaypoint()
    {
        if (_waypoints <= 0)
        {
            return;
        }
        if (NextWaypointIndex < 0)
        {
            return;
        }

        var distance = Vector3.Distance(this.transform.position, Waypoints[NextWaypointIndex]);
        if (distance <= DistanceToWaypointThreshold)
        {
            //finished travelling to a waypoint, reset the flag
            _travellingToNextWaypoint = false;

            if (NextWaypointIndex + 1 < _waypoints)
            {
                NextWaypointIndex++;
            }
            else
            {
                NextWaypointIndex = -1;
            }
            InvokeOnPathProgress(NextWaypointIndex, _waypoints, distance);
        }
    }


}
