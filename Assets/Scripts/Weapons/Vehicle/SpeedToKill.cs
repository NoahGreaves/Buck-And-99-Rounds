using UnityEngine;

public class SpeedToKill : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    private float _originalDamage;

    private void Start()
    {
        _originalDamage = _damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Player.CanSpeedKill || collision.gameObject.layer == Player.LAYER)
            return;

        DealDamage(collision.gameObject);
    }

    private void DealDamage(GameObject objectToDamage) 
    {
        var collisionHP = objectToDamage.GetComponent<Health>();
        if (collisionHP == null)
            return;
        collisionHP.TakeDamage(_damage);
    }

    // use to increase/decrease damage with abilities
    public void ChangeDamage(float newDmg) 
    {
        _damage = newDmg;
    }

    private void ResetDamage() 
    {
        _damage = _originalDamage;
    }
}
