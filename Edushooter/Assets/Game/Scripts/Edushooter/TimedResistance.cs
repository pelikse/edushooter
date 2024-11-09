using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedResistance : MonoBehaviour
{
    public EnemyHealth Health;
    public GameObject ResistanceIndicator;

    [Space]

    public bool IgnoreResistance = false;

    protected bool _resistanceActive = false;
    protected float _lastTimestamp = 0f;
    protected float _previousDamageMultiplier = 1f;
    protected float _resistanceDuration = 0f;

    private void Start()
    {
        if (Health == null)
        {
            Health = GetComponent<EnemyHealth>();
        }
    }

    private void Update()
    {
        if (_resistanceActive)
        {
            // if resistance duration is over, turn off resistances
            if (Time.time - _lastTimestamp > _resistanceDuration)
            {
                Health.DamageMultiplier = _previousDamageMultiplier;
                _resistanceActive = false;
                ResistanceIndicator?.SetActive(false);
            }
        }
    }

    // add damage resistance in the form of a multiplier to the current damage multiplier
    public void AddResistance(float duration, float multiplier)
    {
        _lastTimestamp = Time.time;
        _resistanceDuration = duration;

        //if we add resistance even though we're already in a state of resistance, just refresh the timer and stop
        if (_resistanceActive)
        {
            return;
        }

        _previousDamageMultiplier = Health.DamageMultiplier;
        ResistanceIndicator?.SetActive(true);
        Health.DamageMultiplier *= multiplier;
        _resistanceActive = true;
    }
}
