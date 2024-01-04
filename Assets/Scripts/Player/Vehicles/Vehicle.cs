using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private Rigidbody _playerRB;
    public Rigidbody PlayerRb {  get => _playerRB; }

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

    private void Update()
    {
        CurrentPlayerVelocity = _playerRB.velocity.sqrMagnitude;
    }

    public void SetPosition(Vector3 position)
    {
        _playerRB.position = position;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _playerRB.velocity = velocity;
    }
}
