using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float _timeToExplode = 1f;
    [SerializeField] private float _countdownAmount = 1f;

    [SerializeField] private Transform _explosionTransform;
    [SerializeField] private float _explosionPower;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionUpwardForce = 3f;
    [SerializeField] private float _explosionDamage = 10f;

    private float _currentTimeToExplode;
    private bool _hasExploded = false;

    private void OnEnable()
    {
        _currentTimeToExplode = _timeToExplode;
        _explosionTransform = transform;
    }

    private void Update()
    {
        _currentTimeToExplode -= Time.deltaTime * _countdownAmount;
        if (_currentTimeToExplode > 0)
            return;

        if (!_hasExploded)
            _hasExploded = true;

        Explode(_explosionTransform.position, _explosionPower, _explosionRadius, _explosionUpwardForce, _explosionDamage);
    }

    private void Explode(Vector3 explosionPosition, float explosionPower, float explosionRadius, float explosionUpwardForce, float explosionDamage)
    {
        // Get Objects within a radius, and tell them the Explosion has gone off
        GameEvents.Explosion(explosionPosition, explosionPower, explosionRadius, explosionUpwardForce, explosionDamage);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_explosionTransform.position, _explosionRadius);
    }
#endif
}