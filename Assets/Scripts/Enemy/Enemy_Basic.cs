using UnityEngine.AI;
using UnityEngine;

public class Enemy_Basic : Enemy
{
    [SerializeField] protected Weapon_Pistol _currentWeapon;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 25f;
    [SerializeField] private Transform[] _points;
    private int destPoint = 0;

    private NavMeshAgent _agent;
    private Transform _currentDestination;

    private void Awake()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.autoBraking = false; // stops pausing between destination points
    }

    protected override void Start()
    {
        base.Start();
        _agent.destination = _points[0].position; // Set Agent's initial destination as the first index of the array
    }

    protected override void Update()
    {
        base.Update();
        Move();

        bool shootWeapon = CheckToShootWeapon();
        Debug.Log(shootWeapon);
        if (shootWeapon && !_currentWeapon.IsOnCooldown)
        {
            _currentWeapon.ShootWeapon();
        }
    }

    protected override void Move()
    {
        base.Move();

        //_agent.SetDestination();
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            ChangeDestination();

    }

    private void ChangeDestination() 
    {
        if (_points.Length == 0)
            return;

        _agent.destination = _points[destPoint].position;

        // Choose the next point in the array as the destination, cycling to the start if necessary.
        destPoint = (destPoint + 1) % _points.Length;
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
        bool aimedAtCorrectTarget = hit.transform.gameObject == PlayerTarget;
        bool canShoot = aimedAtTarget && aimedAtCorrectTarget;

        // REFACTOR --> MAKE THE ENEMY STOP MOVING AND SHOOT THE PLAYER, MAKE THE ENEMY CHASE PLAYER IF PLAYER LEAVE FOV RADIUS?
        if (canShoot)
        {
            _agent.isStopped = true;
        }

        return canShoot;
    }
}
