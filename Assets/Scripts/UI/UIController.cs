using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Space(10)]
    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI _playerVelocity;
    [SerializeField] private TextMeshProUGUI _playerHealth;
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private TextMeshProUGUI _playerFuel;
    [SerializeField] private Slider _fuelSlider; 

    [Space(10)]
    [Header("Level Stats")]
    [SerializeField] private TextMeshProUGUI _numOfEnemiesRemaining;
    [SerializeField] private TextMeshProUGUI _startingCountdown;

    [Space(10)]
    [Header("Screens")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _debugScreen;

    /// <summary>
    /// MAKE A DEBUG MENU
    /// </summary>

    private void OnEnable()
    {
        SubscribeToEvents();
        UpdatePlayerLivesUI(Player.NumOfLives);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    #region EventSubscription
    private void SubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath += PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate += UpdateHealthUI;
        GameEvents.OnPlayerLivesUpdate += UpdatePlayerLivesUI;
        GameEvents.OnPlayerElimination += PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate += UpdateVelocityUI;
        GameEvents.OnPlayerFuelUpdate += UpdateFuelAmount;

        GameEvents.OnEnemyCountUpdate += EnemyCountUI;

        GameEvents.OnCountdownUpdate += Countdown;
    }

    private void UnsubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath -= PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate -= UpdateHealthUI;
        GameEvents.OnPlayerLivesUpdate -= UpdatePlayerLivesUI;
        GameEvents.OnPlayerElimination -= PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate -= UpdateVelocityUI;
        GameEvents.OnPlayerFuelUpdate -= UpdateFuelAmount;

        GameEvents.OnEnemyCountUpdate -= EnemyCountUI;

        GameEvents.OnCountdownUpdate -= Countdown;
    }
    #endregion

    private void UpdateHealthUI(float newHealth) 
    {
        _playerHealth.text = $"{newHealth}HP";
    }

    private void UpdatePlayerLivesUI(int numOfLives)
    {
        _playerLives.text = $"{numOfLives}";
    }

    private void UpdateVelocityUI(float newVelocity)
    {
        _playerVelocity.text = $"{(int)newVelocity}km/hr";
    }

    private void UpdateFuelAmount(float fuelAmount)
    {
        _playerFuel.text = $"Fuel: {(int)fuelAmount}";
        _fuelSlider.value = fuelAmount;
    }

    // Count Enemies in Current Level
    private void EnemyCountUI(int enemyCount)
    {
        _numOfEnemiesRemaining.text = $"Enemies: {enemyCount}";

        PlayerObjectiveCompleteUI();
    }

    public void SetCountdownEnabled(bool enabled) => _startingCountdown.gameObject.SetActive(enabled);
    private void Countdown(int newTime) 
    {
        _startingCountdown.text = $"{newTime}";
    }

    // Count Total Kill Count for Play Session
    private void PlayerEliminationUI() {}

    private void PlayerObjectiveCompleteUI()
    {
    }

    private void PlayerDeathUI()
    {
        Time.timeScale = 0f;
        SetGameOverScreen(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetGameOverScreen(bool isActive)
    {
        _gameOverScreen.SetActive(isActive);
    }

    #region Button Presses

    public void OnGameOverRestartPressed() 
    {
        SetGameOverScreen(false);
        Time.timeScale = 1f;
    }

    #endregion
}