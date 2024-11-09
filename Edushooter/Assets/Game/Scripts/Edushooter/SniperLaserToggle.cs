using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperLaserToggle : MonoBehaviour
{
    private Weapon _weapon;
    private WeaponLaserSight _weaponLaserSight;

    public void ToggleWeapon(bool on)
    {
        if (_weaponLaserSight == null)
        {
            GetLaser();
            return;
        }

        if (on)
        {
            _weaponLaserSight.LaserMaxDistance = 75;
        }
        else
        {
            _weaponLaserSight.LaserMaxDistance = 0;
        }
    }

    private void GetLaser()
    {
        _weapon = GetComponent<CharacterHandleWeapon>().CurrentWeapon;

        if (_weapon != null)
        {
            _weaponLaserSight = _weapon.GetComponent<WeaponLaserSight>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetLaser();
    }
}
