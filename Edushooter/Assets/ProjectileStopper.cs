using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStopper : MonoBehaviour
{
    public Projectile Projectile;

    private bool _initialized = false;

    private float _initialSpeed = 0f;
    private float _initialAcceleration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _initialized = Projectile != null;
        
        if (_initialized)
        {
            _initialSpeed = Projectile.Speed;
            _initialAcceleration = Projectile.Acceleration;
        }
    }

    public void StopProjectile()
    {
        Projectile.Speed = 0;
        Projectile.Acceleration = 0;
    }

    public void ResumeProjectile()
    {
        Projectile.Speed = _initialSpeed;
        Projectile.Acceleration = _initialAcceleration;
    }
}
