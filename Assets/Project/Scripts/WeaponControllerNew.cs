using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerNew : MonoBehaviour
{
    public Transform weaponHolder;
    public Weapon startingWeapon;
    Weapon equippedWeapon;

    private void Start()
    {
        if(startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
    }

    public void EquipWeapon(Weapon weaponToEquip)
    {
        if(equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = Instantiate(weaponToEquip, weaponHolder.position, weaponHolder.rotation) as Weapon;
        equippedWeapon.transform.parent = weaponHolder;
    }

    public void GunFire()
    {
        if(equippedWeapon != null)
        {
            equippedWeapon.GunFire();
        }
    }

    public void Reload()
    {
        if(equippedWeapon != null)
        {
            equippedWeapon.Reload();
        }
    }

    public float GunHeight
    {
        get
        {
            return weaponHolder.position.y;
        }
    }
}