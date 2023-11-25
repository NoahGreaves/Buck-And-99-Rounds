using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private float _health = 100;
    [SerializeField] private float _fuelAmount = 10f;

    private bool _isAlive = true;
    private Objective _objective;

    private bool _canTakeDamage = true;

    private float _currentHealth;
    private float CurrentHealth
    { 
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_isPlayer)
                GameEvents.PlayerHealthUpdate(_currentHealth);

            if (_currentHealth <= 0)
                _isAlive = false;
        }
    }

    private void Awake()
    {
        if (_isPlayer)
        { 
            Player.Health = this;
            return;
        }

        _objective = GetComponent<Objective>();
    }

    private void Start()
    {
        CurrentHealth = _health;
    }

    public void TakeDamage(float damage)
    {
        // If the player speed can kill, dont deal damager to player
        if (_isPlayer && Player.CanSpeedKill)
            return;

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            Death();
    }

    private void RestoreHP(float hpAmount, bool fullHP = false)
    {
        // if fullHP is true, set to the max health, else restore the hpAmount
        CurrentHealth = fullHP ? _health : _currentHealth + hpAmount;
    }

    // TODO: Implement proper death ( animations, audio, vfx )
    public void Death() 
    {
        if (_isPlayer)
        { 
            PlayerDeath();
            return;
        }

        _isAlive = false;
        GameEvents.PlayerElimination();

        // Give player fuel on kill
        GameEvents.AddPlayerFuel(_fuelAmount);

        _objective.SetIsCompleted(true);

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
        GameEvents.PlayerDeath();

        // Destroy(gameObject);
    }
}
