using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class Enemy_Basic : Enemy
{
    [SerializeField] protected Weapon_Pistol _currentWeapon;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 25f;
    [SerializeField] private float _targetDistanceThreshold = 0.5f;

    private Vehicle _playerVehicle;

    private int destPoint = 0;

    private Transform _currentDestination;

    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // stops pausing between destination points
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();

        bool shootWeapon = CheckToShootWeapon();
        Debug.Log(shootWeapon);
        if (_currentWeapon != null && !_currentWeapon.IsOnCooldown && shootWeapon)
        {
            _currentWeapon.ShootWeapon();
        }

    }

    protected override void Move()
    {
        base.Move();

        // Get Player Vehicle and Set the Agents Destination
        var playerVehicle = Player.CurrentPlayerVehicle;
        var targetPosition = playerVehicle.transform.position;
        agent.SetDestination(targetPosition);
        

        if (!agent.pathPending && agent.remainingDistance < _targetDistanceThreshold)
            ChangeDestination();

    }

    private void ChangeDestination()
    {
    }

    // Make sure enemy is facing a Target before shooting
    // If NO target is available in range, DON'T SHOOT
    protected override bool CheckToShootWeapon()
    {
        return base.CheckToShootWeapon();

        //// Raycast forward, if cast collides with Player/Decoy return true
        //RaycastHit hit;
        //bool aimedAtTarget = Physics.Raycast(transform.position, transform.forward, out hit, fov.GetViewRadius, fov.GetTargetMask);
        //if (!aimedAtTarget)
        //    return false;
        //bool aimedAtCorrectTarget = hit.transform.gameObject == playerTarget;
        //bool canShoot = aimedAtTarget && aimedAtCorrectTarget;

        //// REFACTOR --> MAKE THE ENEMY STOP MOVING AND SHOOT THE PLAYER, MAKE THE ENEMY CHASE PLAYER IF PLAYER LEAVE FOV RADIUS?
        //if (canShoot)
        //{
        //    agent.isStopped = true;
        //}

        //return canShoot;
    }
}
