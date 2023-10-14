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

    private float _lifetimeCount = 0f;
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

        var collisionHP = collision.gameObject.GetComponent<Health>();
        if (collisionHP == null)
            return;
        collisionHP.TakeDamage(Damage);
        Despawn();
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