using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class Enemy_Basic : Enemy
{
    [SerializeField] protected Weapon_Pistol _currentWeapon;

    [Header("Movement")]
    //[SerializeField] private float _movementSpeed = 25f;
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _targetDistanceThreshold = 0.5f;

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
        agent.destination = _points[0].position; // Set Agent's initial destination as the first index of the array
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

        if (!agent.pathPending && agent.remainingDistance < _targetDistanceThreshold)
            ChangeDestination();

    }

    private void ChangeDestination()
    {
        if (_points.Length == 0)
            return;


        // Choose the next point in the array as the destination, cycling to the start if necessary.
        destPoint = (destPoint + 1) % _points.Length;
        agent.destination = _points[destPoint].position;
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
