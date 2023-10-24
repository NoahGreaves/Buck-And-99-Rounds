using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private float _health = 100;

    private bool _isAlive = true;

    private float _currentHealth;
    private float CurrentHealth
    { 
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            GameEvents.PlayerHealthUpdate(_currentHealth);

            if (_currentHealth <= 0)
                _isAlive = false;
        }
    }

    private void Awake()
    {
        if (_isPlayer)
            Player.Health = this;
        SubscribeToEvents();
    }

    private void Start()
    {
        CurrentHealth = _health;
        GameEvents.GetEnemies(true);
    }

    private void SubscribeToEvents() 
    {
        GameEvents.OnGetEnemies += CountEnemies;
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
        GameEvents.GetEnemies(false);

        // play death sounds


        // animation?? 


        UnsubscribeToEvents();

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

    private void UnsubscribeToEvents()
    {
        //GameEvents.OnCountEnemies -= CountEnemies;
    }

    private void CountEnemies(bool isAlive) 
    {
        if (_isPlayer)
            return;

        // call EnemeyCountUI using the GameEvents. Find a way to get 
        // the current enemy count from either health, GameEvent, or EliminationObjective

        print(_isPlayer);
        ObjectiveData.CountObjectives(isAlive);
        //GameEvents.CountEnemies(isAlive);
    }
}
