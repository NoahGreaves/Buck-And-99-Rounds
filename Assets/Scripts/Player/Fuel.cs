using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float _fuelAmount = 100;
    [SerializeField] private float _usage = 1;
    [SerializeField] private Health _playerHealth;
     
    private float _currentFuelAmount;
    private bool _useFuel = true;

    private void Start()
    {
        _currentFuelAmount = _fuelAmount;
    }

    private void Update()
    {
        if (Player.IsMoving && _useFuel)
        {
            _currentFuelAmount -= _usage * Time.deltaTime;
        }

        // if player runs out of fuel, player dies
        if (_currentFuelAmount <= 0)
        {
            _playerHealth.Death();
        }
        //print(_currentFuelAmount);

        // if player eliminates enemy, gain fuel
        //if () { }
    }

    public void AddFuel(float fuelAmount)
    {
        _useFuel = false;
        _currentFuelAmount += fuelAmount;
        _useFuel = true;
    }
}
