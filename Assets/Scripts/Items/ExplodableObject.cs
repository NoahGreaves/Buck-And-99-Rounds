using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    private Rigidbody _RB;
    private Health _health;

    private void OnEnable()
    {
        GameEvents.OnExplosion += OnExplosion;

        _RB = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
    }

    private void OnDisable()
    {
        GameEvents.OnExplosion -= OnExplosion;
    }

    private void OnExplosion(Vector3 explosionPosition, float explosionPower, float explosionRadius, float explosionUpwardForce, float explosionDamage) 
    {
        _RB.AddExplosionForce(explosionPower, explosionPosition, explosionRadius, explosionUpwardForce, ForceMode.Impulse);

        if (_health == null)
            return;
        _health.TakeDamage(explosionDamage);
    }
}