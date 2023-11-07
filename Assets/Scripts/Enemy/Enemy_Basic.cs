using UnityEngine.AI;
using UnityEngine;

public class Enemy_Basic : Enemy
{
    [Header("Movement")]
    [SerializeField] private float _targetDistanceThreshold = 0.5f;

    [Header("Damage")]
    [SerializeField] protected Weapon _currentWeapon;
    [SerializeField] private float _damageOnContact = 10f;

    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();

        var x = GetComponent<Rigidbody>().velocity.sqrMagnitude;

        bool shootWeapon = CheckToShootWeapon();
        if (_currentWeapon != null && !_currentWeapon.IsOnCooldown && shootWeapon)
        {
            _currentWeapon.PlayerWeaponAttack();
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
