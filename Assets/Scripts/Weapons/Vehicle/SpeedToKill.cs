using UnityEngine;

public class SpeedToKill : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;

    private const int PLAYER_LAYER = 1 << 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (!Player.CanSpeedKill && collision.gameObject.layer == PLAYER_LAYER)
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
}
