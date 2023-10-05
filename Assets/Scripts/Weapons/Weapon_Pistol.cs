using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Pistol : Weapon
{
    private void Awake()
    {
        WEAPONTYPE = WeaponType.GUN_PISTOL;
    }

    public override void ShootWeapon() 
    {
        base.ShootWeapon();
    }

    protected override void Fire(Vector3 weaponDirection)
    {
        base.Fire(weaponDirection);
        var bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        bullet.transform.forward = weaponDirection;
        bullet.StartCountdown();
    }
}
