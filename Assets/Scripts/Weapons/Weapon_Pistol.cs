using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Pistol : Weapon
{
    [SerializeField] private Transform _projectileSpawnPoint;

    private void Awake()
    {
        WEAPONTYPE = WeaponType.RANGED;
    }

    protected override IEnumerator Cooldown(float cooldown)
    {
        yield return base.Cooldown(cooldown);
    }

    protected override void Fire(Vector3 weaponDirection)
    {
        base.Fire(weaponDirection);
        var bullet = Instantiate(bulletPrefab, _projectileSpawnPoint.position, bulletPrefab.transform.rotation);
        bullet.transform.forward = weaponDirection;
        bullet.StartCountdown();
    }
}
