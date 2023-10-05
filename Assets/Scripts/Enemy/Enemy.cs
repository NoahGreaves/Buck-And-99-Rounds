using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Class For All Enemies
public class Enemy : MonoBehaviour
{
    protected FieldOfView fov;

    [SerializeField] protected float _rotationSpeed = 2f;

    protected static float MovementSpeed = 100;
    private static float _defaultMovementSpeed = 100;

    [SerializeField] protected float Health = 100;
    private static readonly float _defaultHealth = 100;

    private List<Transform> _decoyList = new List<Transform>();
    protected Transform targetPlayer;

    protected GameObject target;
    protected bool isLookingAtTarget = false;

    public static float SetMoveSpeed(float speed) => MovementSpeed = speed;
    public static float ResetMoveSpeed(float speed) => MovementSpeed = speed;

    public void ResetHealth() => Health = _defaultHealth;
    public void ResetMovementSpeed() => MovementSpeed = _defaultMovementSpeed;

    protected virtual void Start()
    {
        fov = GetComponent<FieldOfView>();
    }

    protected virtual void Update()
    {
        var targets = fov.GetVisibleTargets;
        if (targets.Count == 0)
            return;

        _decoyList.Clear(); // TODO: SORT DECOY LIST FROM CLOSTEST TO FARTHEST FROM PLAYER

        GetTargets(targets);
        var priorityTarget = ChoosePriorityTarget();

        if (priorityTarget == null)
            return;

        target = priorityTarget.gameObject;
        RotateToLookAt(priorityTarget);
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
            PlayerTypeObject pto = targets[i].gameObject.GetComponent<PlayerTypeObject>();
            PlayerType possiblePlayer = pto ? pto.PLAYERTYPE : PlayerType.NULL;
            switch (possiblePlayer)
            {
                case PlayerType.DECOY:
                    _decoyList.Add(targets[i]);
                    continue;
                case PlayerType.PLAYER:
                    targetPlayer = targets[i];
                    continue;
                default:
                    Debug.LogError("ENEMY.CS -> FOV DETECTS NULL PLAYER");
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

    public void OnProjectileCollision(Projectile projectile)
    {
        
    }

    protected virtual bool CheckToShootWeapon()
    {
        return false;
    }

    protected virtual void ShootWeapon() 
    {
        
    }
}