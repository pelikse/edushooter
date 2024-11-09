using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public WeaponType Weapon;
    public WeaponSelectManager WeaponSelectManager;

    public void SetSelectedWeapon()
    {
        WeaponSelectManager.SelectWeapon(Weapon);
    }
}
