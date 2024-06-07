using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMode
{
    FOLLOW_PLAYER = 0,
    FOLLOW_NODES
}

public class AIVehicleController : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private AIMode _aiMode;

    [Header("Driving Settings")]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _maxSpeed;

    [Space(10)]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _currentSpeed = 1f; // (-1, 1)

    private Rigidbody _rb;
    private Transform _playerTransform;
    private Vector3 _targetPosition = Vector3.zero;
    private Vector2 _inputVector = Vector2.zero;

    public Vector3 CurrentTargetPosition
    {
        get => _targetPosition;
    }

    public float TurnSpeed
    {
        get => _turnSpeed;
    }

    private void Awake()
    {
        _rb = gameObject.GetComponentInChildren<Rigidbody>();
        _playerTransform = Player.CurrentPlayerVehicle.transform;
    }

    private void Update()
    {
        _model.transform.position = _rb.transform.position;

        switch (_aiMode)
        {
            case AIMode.FOLLOW_PLAYER:
                FollowPlayer();
                break;
            case AIMode.FOLLOW_NODES:
                break;
        }
    }

    private void FixedUpdate()
    {
        _inputVector.y = _currentSpeed;
        MoveVehicle(_inputVector.y);
    }

    private void FollowPlayer()
    {
        if (_playerTransform == null)
            return;

        _targetPosition = _playerTransform.position;
    }

    private void MoveVehicle(float moveInput)
    {
        var speed = moveInput * _moveSpeed;

        // Apply forces to simulate car movement.
        Vector3 moveForce = speed * _model.transform.forward;
        _rb.AddForce(moveForce, ForceMode.Acceleration);
    }
}
