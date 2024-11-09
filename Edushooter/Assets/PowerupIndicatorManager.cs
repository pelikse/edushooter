using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupIndicatorManager : MMSingleton<PowerupIndicatorManager>
{
    public Transform Pivot, Indicator;
    public GameObject Arrow;
    [Space]
    public float UpdateFrequency = 0.2f, RotationSpeed = 5f;

    public List<Transform> availablePowerups = new List<Transform>();
    [MMReadOnly][SerializeField] private Vector3 targetPowerupLocation;
    private bool _hasActivePowerups;
    private float _lastUpdate = 0f;


    public void AddActivePowerup(GameObject powerup)
    {
        Debug.Log("adding indicator for powerup " + powerup.name);

        availablePowerups.Add(powerup.transform);
        SetTargetPowerup(powerup.transform.position);

        SnapIndicator(Pivot.position, powerup.transform.position);
        Arrow.SetActive(true);

        _hasActivePowerups = true;
        _lastUpdate = Time.time;
    }

    public void RemovePowerup(GameObject powerup)
    {
        Debug.Log("removed active powerup from indicator");
        availablePowerups.Remove(powerup.transform);

        if (availablePowerups.Count > 0)
        {
            SetTargetPowerup(availablePowerups.First().position);
        }
        else
        {
            _hasActivePowerups = false;
            Arrow.SetActive(false);
        }

    }

    private void SetTargetPowerup(Vector3 target)
    {
        targetPowerupLocation = target;
    }
    private void RotateIndicator(Vector3 pivotPosition, Vector3 targetPosition)
    {
        // Calculate the direction from pivot to target
        Vector3 direction = targetPosition - pivotPosition;

        // Calculate the rotation required to face the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Interpolate between the current rotation and the target rotation
        Indicator.transform.rotation = Quaternion.Slerp(Indicator.transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
    }

    private void SnapIndicator(Vector3 pivotPosition, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - pivotPosition;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Indicator.transform.rotation = rotation;
    }

    private void Start()
    {
        Arrow.SetActive(false);
    }

    private void Update()
    {
        if (_hasActivePowerups)
        {

            RotateIndicator(Pivot.position, targetPowerupLocation);
        
        }
    }
}
