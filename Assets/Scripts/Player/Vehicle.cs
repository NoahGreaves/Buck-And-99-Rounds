using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private Rigidbody _playerRB;

    private float _currentPlayerVelocity;
    private float CurrentPlayerVelocity
    {
        //get => _currentPlayerVelocity;
        set
        {
            _currentPlayerVelocity = value;
            GameEvents.PlayerVelocityUpdate(_currentPlayerVelocity);
        }
    }

    private void Awake()
    {
        Player.CurrentPlayerVehicle = this;
        _playerRB = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        CurrentPlayerVelocity = _playerRB.velocity.sqrMagnitude;
    }
}
