using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name = "Test Shitter";

    public virtual WeaponType WEAPONTYPE { get; protected set; }

    //protected float fireRate = 4f;
    //protected float reloadSpeed = 10f;

    [SerializeField] protected Projectile bulletPrefab;
    [SerializeField] protected float cooldown = 1.5f; // 1.5 seconds of cooldown between weapon shots/attacks

    private float _cooldownTime;

    private bool _isOnCooldown = false;
    public bool IsOnCooldown { get => _isOnCooldown; }

    // Called From `PlayerShooting` for Player Controls
    public virtual void ShootWeapon() 
    {
        if (IsOnCooldown)
        {
            return;
        }
        
        Fire(transform.forward);
        StartCoroutine(Cooldown());
    }
    
    protected virtual void Fire(Vector3 weaponDirection) { }

    private void Start()
    {
        _cooldownTime = cooldown;
    }

    protected virtual IEnumerator Cooldown() 
    {
        _isOnCooldown = true;

        do
        {
            _cooldownTime -= Time.deltaTime;
            yield return null;

        } while (_cooldownTime > 0);

        _isOnCooldown = false;
        _cooldownTime = cooldown;
        StopCoroutine(Cooldown());

        yield return null;
    }
}
