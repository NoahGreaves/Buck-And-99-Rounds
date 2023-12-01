using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : Enemy
{
    [SerializeField] protected Weapon _currentWeapon;
    [SerializeField] private bool _isStationary;

    protected override void Update()
    {
        base.Update();
        Move();

        bool shootWeapon = CheckToShootWeapon();
        if (_currentWeapon != null && !_currentWeapon.IsOnCooldown && shootWeapon)
        {
            _currentWeapon.PlayerWeaponAttack();
        }
    }

    protected override void Move()
    {
        base.Move();

        // Get Player Vehicle from FOV and set destination
        GetTargets(Fov.VisibleTargets);
        if (targetPlayer == null)
            return;
        RotateToLookAt(targetPlayer);

        if (_isStationary)
            return;

        //agent.SetDestination(targetPlayer.transform.position);


        // if enemy is moving between multiple points
        //if (!agent.pathPending && agent.remainingDistance < _targetDistanceThreshold)
        //    ChangeDestination();
    }

    // Make sure enemy is facing a Target before shooting
    // If NO target is available in range, DON'T SHOOT
    protected override bool CheckToShootWeapon()
    {
        return base.CheckToShootWeapon();
    }
}
