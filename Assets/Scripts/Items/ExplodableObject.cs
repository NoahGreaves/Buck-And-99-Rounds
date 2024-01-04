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
        double explosionDistance = (transform.position - explosionPosition).sqrMagnitude;
        bool isInExplosionRadius = explosionDistance <= ( explosionRadius * explosionRadius );
        bool healthIsNull = _health == null;
        bool noDmg = explosionDamage <= 0;

        if (!isInExplosionRadius || healthIsNull || noDmg)
          return;
        
        _RB.AddExplosionForce(explosionPower, explosionPosition, explosionRadius, explosionUpwardForce, ForceMode.Impulse);
        _health.TakeDamage(explosionDamage);
    }
}