using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private float _health = 100;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _health;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"Health: {_currentHealth}");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void RestoreHP(float hpAmount, bool fullHP = false)
    {
        // if fullHP is true, set to the max health, else restore the hpAmount
        _currentHealth = fullHP ? _health : _currentHealth + hpAmount;
    }

    // TODO: Implement proper death ( animations, audio, vfx )
    private void Die() 
    {
        if (_isPlayer)
        { 
            PlayerDeath();
            return;
        }

        // play death sounds


        // animation?? 


        // Destroy Object
        Destroy(gameObject);
    }

    private void PlayerDeath() 
    {
        // sounds

        // animation

        // PROPER game over
        Destroy(gameObject);
    }
}
