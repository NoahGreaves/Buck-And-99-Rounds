using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private Rigidbody _playerRB;

    private float _currentPlayerVelocity;
    public float CurrentPlayerVelocity
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

    private void OnEnable()
    {
        GameManager.PlayerVehicle = this;
    }

    private void Update()
    {
        CurrentPlayerVelocity = _playerRB.velocity.sqrMagnitude;
    }
}
