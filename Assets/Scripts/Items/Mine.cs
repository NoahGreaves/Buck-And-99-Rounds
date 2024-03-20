using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _explosionUpwardForce = 5f;
    [SerializeField] private float _armTime = 1f;

    private bool _canExplode = false;

    private void Start()
    {
        StartCoroutine(ArmTimer());
    }

    private IEnumerator ArmTimer() 
    {
        while (_armTime >=0)
        {
            _armTime -= Time.deltaTime;
            if (_armTime <= 0)
            {
                break;
            }
            yield return null;
        }

        _canExplode = true;
        StopCoroutine(ArmTimer());
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_canExplode)
            return;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius);

        for (int i = 0; i < colliders.Length - 1; i++)
        {
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();

            if (rb == null)
                continue;

            rb.AddExplosionForce(_explosionForce, explosionPos, _explosionRadius, _explosionUpwardForce, ForceMode.Impulse);
            DestroyMine();
        }
    }

    private void DestroyMine() 
    {
        Destroy(gameObject);
    }
}
