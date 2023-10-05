using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Basic : Enemy
{
    [SerializeField] protected Weapon_Pistol _currentWeapon;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        bool shootWeapon = CheckToShootWeapon();
        if (shootWeapon && !_currentWeapon.IsOnCooldown)
        {
            _currentWeapon.ShootWeapon();
        }
    }

    // Make sure enemy is facing a Target before shooting
    // If NO target is available in range, DON'T SHOOT
    protected override bool CheckToShootWeapon()
    {
        base.CheckToShootWeapon();

        // Raycast forward, if cast collides with Player/Decoy return true
        RaycastHit hit;
        bool aimedAtTarget = Physics.Raycast(transform.position, transform.forward, out hit, fov.GetViewRadius, fov.GetTargetMask);
        if (!aimedAtTarget)
            return false;
        bool aimedAtCorrectTarget = hit.transform.gameObject == target;
        bool canShoot = aimedAtTarget && aimedAtCorrectTarget;
        return canShoot;
    }
}
