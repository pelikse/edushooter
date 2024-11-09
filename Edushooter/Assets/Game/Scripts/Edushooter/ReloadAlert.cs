using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAlert : MonoBehaviour
{
    public ProjectileWeapon Weapon;

    [Space]

    public float AlertPercentage; // at what percentage should we start alerting to reload?

    [SerializeField]
    [MMReadOnly]
    private int MagazineSize;

    private int _currentAmmo;
    private int _alertThreshold;

    // Start is called before the first frame update
    void Start()
    {
        if (Weapon == null)
        {
            Weapon = gameObject.GetComponent<ProjectileWeapon>();
        }
    
        MagazineSize = Weapon.MagazineSize;
        _currentAmmo = MagazineSize;

        _alertThreshold = (int)(MagazineSize * AlertPercentage);
    }

    // Update is called once per frame
    void Update()
    {
        //whenever ammo changes
        if (_currentAmmo != Weapon.CurrentAmmoLoaded)
        {
            _currentAmmo = Weapon.CurrentAmmoLoaded;

            //if we're under the threshold
            if (_currentAmmo <= _alertThreshold) {
                MMGameEvent.Trigger("ReloadAlertTrigger");
            }
        }
    }
}
