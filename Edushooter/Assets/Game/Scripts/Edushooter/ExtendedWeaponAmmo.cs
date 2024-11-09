using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWeaponAmmo : WeaponAmmo
{
    protected override void ConsumeAmmo()
    {
        if (_weapon.MagazineBased)
        {
            _weapon.CurrentAmmoLoaded = _weapon.CurrentAmmoLoaded - _weapon.AmmoConsumedPerShot;
        }
        else
        {
            for (int i = 0; i < _weapon.AmmoConsumedPerShot; i++)
            {
                AmmoInventory.UseItem(AmmoID);
                CurrentAmmoAvailable--;
            }
        }

        if (CurrentAmmoAvailable < _weapon.AmmoConsumedPerShot)
        {
            if (_weapon.AutoDestroyWhenEmpty)
            {
                StartCoroutine(_weapon.WeaponDestruction());
            }
        }
    }
}
