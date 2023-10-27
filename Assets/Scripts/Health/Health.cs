using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private float _health = 100;

    private bool _isAlive = true;
    private Objective _objective;

    private float _currentHealth;
    private float CurrentHealth
    { 
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            GameEvents.PlayerHealthUpdate(_currentHealth);

            if (_currentHealth <= 0)
            { 
                _isAlive = false;
            }
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
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
            EnemyDeath();
    }

    private void RestoreHP(float hpAmount, bool fullHP = false)
    {
        // if fullHP is true, set to the max health, else restore the hpAmount
        CurrentHealth = fullHP ? _health : _currentHealth + hpAmount;
    }

    // TODO: Implement proper death ( animations, audio, vfx )
    private void EnemyDeath() 
    {
        if (_isPlayer)
        { 
            PlayerDeath();
            return;
        }

        _isAlive = false;
        GameEvents.PlayerElimination();

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
