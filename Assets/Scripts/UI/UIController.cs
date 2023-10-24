using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Space(10)]
    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI _playerVelocity;
    [SerializeField] private TextMeshProUGUI _playerHealth;

    [Space(10)]
    [Header("Level Stats")]
    [SerializeField] private TextMeshProUGUI _numOfEnemiesRemaining;

    [Space(10)]
    [Header("Screens")]
    [SerializeField] private GameObject _gameOverScreen;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath += PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate += UpdateHealthUI;
        GameEvents.OnPlayerElimination += PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate += UpdateVelocityUI;

        // Objective 
        GameEvents.OnObjectiveComplete += PlayerObjectiveCompleteUI;

        GameEvents.OnEnemyCountUpdate += EnemyCountUI;
    }

    private void UnsubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath -= PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate -= UpdateHealthUI;
        GameEvents.OnPlayerElimination -= PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate -= UpdateVelocityUI;
        
        // Objective
        GameEvents.OnObjectiveComplete -= PlayerObjectiveCompleteUI;

        GameEvents.OnEnemyCountUpdate -= EnemyCountUI;
    }

    private void UpdateHealthUI(float newHealth) 
    {
        _playerHealth.text = $"{newHealth}HP";
    }

    private void UpdateVelocityUI(float newVelocity)
    {
        _playerVelocity.text = $"{(int)newVelocity}m/s";
    }

    // Count Enemies in Current Level
    private void EnemyCountUI(int enemyCount)
    {
        print(enemyCount);
        _numOfEnemiesRemaining.text = $"{enemyCount}";
    }

    // Count Total Kill Count for Play Session
    private void PlayerEliminationUI() {}

    private void PlayerObjectiveCompleteUI(Objective objective) { }

    private void PlayerDeathUI()
    {
        Time.timeScale = 0f;
        _gameOverScreen.SetActive(!_gameOverScreen.activeSelf);
    }
}