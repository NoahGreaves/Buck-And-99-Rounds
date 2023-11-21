using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Space(10)]
    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI _playerVelocity;
    [SerializeField] private TextMeshProUGUI _playerHealth;
    [SerializeField] private TextMeshProUGUI _playerFuel;
    [SerializeField] private Slider _fuelSlider; 

    [Space(10)]
    [Header("Level Stats")]
    [SerializeField] private TextMeshProUGUI _numOfEnemiesRemaining;

    [Space(10)]
    [Header("Screens")]
    [SerializeField] private GameObject _gameOverScreen;

    private const float METERS_PER_SECOND_TO_KILOMETERS_PER_HOUR = 3.6f;

    private void OnEnable()
    {
        SubscribeToEvents();
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

    private void SubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath += PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate += UpdateHealthUI;
        GameEvents.OnPlayerElimination += PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate += UpdateVelocityUI;
        GameEvents.OnPlayerFuelUpdate += UpdateFuelAmount;

        GameEvents.OnEnemyCountUpdate += EnemyCountUI;
    }

    private void UnsubscribeToEvents() 
    {
        // Player
        GameEvents.OnPlayerDeath -= PlayerDeathUI;
        GameEvents.OnPlayerHealthUpdate -= UpdateHealthUI;
        GameEvents.OnPlayerElimination -= PlayerEliminationUI;
        GameEvents.OnPlayerVelocityUpdate -= UpdateVelocityUI;
        GameEvents.OnPlayerFuelUpdate -= UpdateFuelAmount;

        GameEvents.OnEnemyCountUpdate -= EnemyCountUI;
    }

    private void UpdateHealthUI(float newHealth) 
    {
        _playerHealth.text = $"{newHealth}HP";
    }

    private void UpdateVelocityUI(float newVelocity)
    {
        newVelocity /= METERS_PER_SECOND_TO_KILOMETERS_PER_HOUR; // converts from m/s to km/hr
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
        _gameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}