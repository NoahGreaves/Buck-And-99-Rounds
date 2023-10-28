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

        GameEvents.OnEnemyCountUpdate += EnemyCountUI;
    }

    private void UnsubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath -= PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate -= UpdateHealthUI;
        GameEvents.OnPlayerElimination -= PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate -= UpdateVelocityUI;

        GameEvents.OnEnemyCountUpdate -= EnemyCountUI;
    }

    private void UpdateHealthUI(float newHealth) 
    {
        _playerHealth.text = $"{newHealth}HP";
    }

    private void UpdateVelocityUI(float newVelocity)
    {
        newVelocity /= 3.6f; // converts from m/s to km/hr
        _playerVelocity.text = $"{(int)newVelocity}km/hr";
    }

    // Count Enemies in Current Level
    private void EnemyCountUI(int enemyCount)
    {
        _numOfEnemiesRemaining.text = $"{enemyCount}";

        PlayerObjectiveCompleteUI();
    }

    // Count Total Kill Count for Play Session
    private void PlayerEliminationUI() {}

    private void PlayerObjectiveCompleteUI()
    {
    }

    private void PlayerDeathUI()
    {
        Time.timeScale = 0f;
        _gameOverScreen.SetActive(!_gameOverScreen.activeSelf);
    }
}