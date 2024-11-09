using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageAfterSeconds : MonoBehaviour
{
    public float Delay = 0.5f;
    public float Damage = 1f;

    public Health Health;

    //cache
    private bool _active = false;
    private float _lastTime = 0f;

    public void StartCountdown()
    {
        _active = true;
        _lastTime = Time.time;
    }

    private void Update()
    {
        if (_active)
        {
            if (Time.time - _lastTime >= Delay)
            {
                if (Health.CanTakeDamageThisFrame())
                {
                    Health.Damage(Damage, gameObject, 0.1f, 0f, gameObject.transform.position);
                    _active = false;
                }
            }
        }
    }
}
