using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{ 
    PLAYER = 0,
    ENEMY
}

public enum ProjectileCollisionType
{
    BREAKABLE_OBSTACLE,
    PLAYER,
    ENEMY
}

public class Projectile : MonoBehaviour
{
    protected Rigidbody RB;

    [SerializeField] protected ProjectileType ProjectileType = ProjectileType.PLAYER;
    [SerializeField] protected float Damage = 100f;
    [SerializeField] protected float Speed = 150f;
    [SerializeField] protected float Lifetime = 3f;

    public float GetDamage => Damage;
    private float _defaultDamage;

    private float _lifetimeCount  = 0f;

    private static readonly LayerMask PLAYER_LAYER_MASK = 3;
    private static readonly LayerMask ENEMY_LAYER_MASK = 6;

    private bool _hasDamaged = false;

    public void ResetLifetimeCooldown() => _lifetimeCount = Lifetime;
    public void ResetDamage() => _defaultDamage = Damage;

    private void Start()
    {
        
        RB = GetComponent<Rigidbody>();
        StartCoroutine(LifetimeCounter());

        _lifetimeCount = Lifetime;
    }

    public void Despawn() 
    {
        // TODO: Change to Object Pooling
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Make sure the projectile only deals damage once
        if (_hasDamaged)
            return;

        // CHECK THE COLLISION OBJECT LAYERMASK --> if Collision LayerMask is ENEMY && ProjectileType is PLAYER ProjectileCollisionType is ENEMY
        // Get Enemy Component and Deal Damage
        // if Collision LayerMask is PLAYER && ProjectileType is ENEMY ProjectileCollisiontType is PLAYER
        // Get Player components and deal damage
        // if Collision LayerMask is NIETHER PLAYER OR ENEMY Set collisionType to BREAKABLE_OBSTACLE --> All Obstacles will be marked as BREAKABLE_OBSTACLE but will have a variable marking if it acutally breaks or not
        LayerMask collidedLayerMask = collision.gameObject.layer;
        ProjectileCollisionType collisionType = collidedLayerMask == ENEMY_LAYER_MASK && ProjectileType == ProjectileType.PLAYER ? ProjectileCollisionType.ENEMY : collidedLayerMask == PLAYER_LAYER_MASK && ProjectileType == ProjectileType.ENEMY ? ProjectileCollisionType.PLAYER : ProjectileCollisionType.BREAKABLE_OBSTACLE;

        switch (collisionType)
        {
            case ProjectileCollisionType.PLAYER:     // PLAYER LAYERMASK
                // Get PlayerTypeObject to determine if projectile hit Decoy or Player
                // implement accordingly
                var playerHP = collision.gameObject.GetComponent<Health>();
                playerHP.TakeDamage(Damage);
                _hasDamaged = true;

                break;

            case ProjectileCollisionType.ENEMY:     // ENEMY LAYERMASK
            { 
                // GET ENEMY COMPONENT AND DEAL DAMAGE
                var enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.OnProjectileCollision(this);
                _hasDamaged = true;
                break;
                }
            case ProjectileCollisionType.BREAKABLE_OBSTACLE:     // BREAKABLE_OBSTACLE LAYERMASK
                break;
            default:
                break;
        }

        if (_hasDamaged)
        {
            Despawn();
        }
    }

    public void StartCountdown() => StartCoroutine(LifetimeCounter());
    private IEnumerator LifetimeCounter()
    {
        _lifetimeCount = Lifetime;
        while (true)
        {
            _lifetimeCount -= Time.deltaTime;
            if (_lifetimeCount <= 0) 
            {
                Despawn();
                break;
            }

            yield return null;
        }

        StopCoroutine(LifetimeCounter());
        yield return null;
    }
}