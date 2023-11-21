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

    protected override void Fire()
    {
        base.Fire();

        // Change to object pooling
        var bullet = Instantiate(bulletPrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        bullet.StartCountdown();
    }
}
