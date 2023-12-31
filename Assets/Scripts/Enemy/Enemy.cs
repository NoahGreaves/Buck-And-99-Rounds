using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

// Base Class For All Enemies
public class Enemy : MonoBehaviour
{
    protected FieldOfView Fov;

    [SerializeField] protected float _rotationSpeed = 2f;

    protected static float MovementSpeed = 100;
    protected const float MIN_DISTANCE = 0.2f;
    private static float _defaultMovementSpeed = 100;

    private List<Transform> _decoyList = new List<Transform>();
    protected Transform targetPlayer;

    protected GameObject PlayerTarget;
    protected bool IsLookingAtTarget = false;

    protected NavMeshAgent agent;

    public static float SetMoveSpeed(float speed) => MovementSpeed = speed;
    public static float ResetMoveSpeed(float speed) => MovementSpeed = speed;

    public void ResetMovementSpeed() => MovementSpeed = _defaultMovementSpeed;

    protected virtual void Start()
    {
        Fov = GetComponent<FieldOfView>();
    }

    protected virtual void Update()
    {
        var targets = Fov.VisibleTargets;
        if (targets.Count == 0)
            return;

        _decoyList.Clear(); // TODO: SORT DECOY LIST FROM CLOSTEST TO FARTHEST FROM PLAYER
    }

    protected void RotateToLookAt(Transform lookAtTarget) 
    {
        // referenced from: https://discussions.unity.com/t/smoothly-rotate-object-towards-target-object/227141
        Vector3 targetDirection = lookAtTarget.position - transform.position;
        Quaternion targetRotation;

        targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion nextRotation = Quaternion.Lerp(transform.localRotation, targetRotation, _rotationSpeed * Time.deltaTime);
        transform.localRotation = nextRotation;
    }

    protected void GetTargets(List<Transform> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            PlayerTypeObject pto = targets[i].GetComponent<PlayerTypeObject>();
            if (pto == null)
            {
                continue;
            }

            switch (pto.PLAYERTYPE)
            {
                case PlayerType.DECOY:
                    _decoyList.Add(targets[i]);
                    continue;
                case PlayerType.PLAYER:
                    targetPlayer = targets[i];
                    continue;
                default:
                    //Debug.LogError("ENEMY.CS -> FOV DETECTS NULL PLAYER");
                    return;
            }
        }
    }

    protected Transform ChoosePriorityTarget()
    {
        if (_decoyList.Count != 0)
        {
            return _decoyList[0];
        }

        return targetPlayer;
    }

    protected virtual bool CheckToShootWeapon()
    {
        // Raycast forward, if cast collides with Player/Decoy return true
        RaycastHit hit;
        bool aimedAtTarget = Physics.Raycast(transform.position, transform.forward, out hit, Fov.GetViewRadius, Fov.GetTargetMask);
        return aimedAtTarget;

        // KEEP INCASE DECOY IS SOMETHING THAT WE WANT TO REIMPLEMENT
        //if (!aimedAtTarget)
        //    return false;
        //bool aimedAtCorrectTarget = hit.transform.gameObject == PlayerTarget;
        //bool canShoot = aimedAtTarget && aimedAtCorrectTarget;

        //// REFACTOR --> MAKE THE ENEMY STOP MOVING AND SHOOT THE PLAYER, MAKE THE ENEMY CHASE PLAYER IF PLAYER LEAVE FOV RADIUS?
        //if (canShoot)
        //{
        //    agent.isStopped = true;
        //}

        //return canShoot;
    }

    public void OnProjectileCollision(Projectile projectile)
    {
    
    }

    protected virtual void ShootWeapon() { }

    protected virtual void Move() { }    
}
