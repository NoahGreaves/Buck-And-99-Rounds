using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name = "Test Shitter";

    [SerializeField] private bool _weaponIsMelee = false;

    [Header("Melee Weapon")]
    [SerializeField] protected float meleeDamage = 10f;
    [SerializeField] protected float meleeCooldown = 1.5f;

    [Header("Ranged Weapon")]
    [SerializeField] protected Projectile bulletPrefab;
    [SerializeField] protected float rangedCooldown = 1.5f; // 1.5 seconds of cooldown between weapon shots/attacks

    private bool _isOnCooldown = false;
    public bool IsOnCooldown { get => _isOnCooldown; }
    public virtual WeaponType WEAPONTYPE { get; protected set; }

    protected void Start()
    {
        SetWeaponType();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (WEAPONTYPE == WeaponType.MELEE)
        {
            var target = collision.gameObject;
            MeleeAttack(target);
        }
    }

    public void PlayerWeaponAttack() 
    {
        if (IsOnCooldown)
            return;
        
        Fire();
        StartCoroutine(Cooldown(rangedCooldown));
    }

    private void SetWeaponType() 
    {
        WEAPONTYPE = _weaponIsMelee ? WeaponType.MELEE : WeaponType.RANGED;
    }
    
    protected virtual void Fire() { }

    protected virtual void MeleeAttack(GameObject target) 
    {
        if (_isOnCooldown && target.layer != Player.LAYER)
            return;

        var health = target.GetComponent<Health>();
        if (health != null)
        {
            print(health.gameObject.name);
            health.TakeDamage(meleeDamage);
        }
        StartCoroutine(Cooldown(meleeCooldown));
    }

    protected virtual IEnumerator Cooldown(float cooldown) 
    {
        _isOnCooldown = true;

        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        _isOnCooldown = false;
        StopCoroutine(Cooldown(cooldown));

        yield return null;
    }
}
