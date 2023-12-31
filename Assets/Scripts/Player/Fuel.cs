using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float _fuelAmount = 100;
    [SerializeField] private float _usage = 1;
    [SerializeField] private Health _playerHealth;

    private float _maxFuelAmount;
    private float _currentFuelAmount;
    private bool _useFuel = true; // use for infinte fuel pick-up

    private void OnEnable()
    {
        _maxFuelAmount = _fuelAmount;
        GameEvents.OnPlayerGivenFuel += AddFuel;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGivenFuel -= AddFuel;
    }

    private void Start()
    {
        _currentFuelAmount = _fuelAmount;
    }

    private void Update()
    {
        if (Player.IsMoving && _useFuel)
        {
            _currentFuelAmount -= _usage * Time.deltaTime;
            GameEvents.PlayerFuelUpdate(_currentFuelAmount);
        }

        // if player runs out of fuel, player dies
        if (_currentFuelAmount <= 0)
        {
            _playerHealth.Death();
        }
    }

    public void SetUseFuel(bool useFuel) => _useFuel = useFuel;

    public void AddFuel(float fuelAmount)
    {
        _currentFuelAmount += fuelAmount;
        if (_currentFuelAmount > _maxFuelAmount)
        {
            _currentFuelAmount = _maxFuelAmount;
        }
    }
}
