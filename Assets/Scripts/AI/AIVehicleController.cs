using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMode
{
    FOLLOW_PLAYER = 0,
    FOLLOW_NODES,
    FOLLOW_PLAYER_NODES
}

public class AIVehicleController : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private AIMode _aiMode;

    [Header("Driving Settings")]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _wallDetectAngle = 180f;

    [Space(10)]
    [SerializeField] private int _numberOfRays;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _yModelOff;
    
    [SerializeField] private float _startWallDetectAngle = -90f;


    [SerializeField] private LayerMask _layerMask;

    private Rigidbody _rb;
    private GameObject _model;
    private float _currentSpeedUsage = 1f;
    private Vector3 _targetPosition = Vector3.zero;
    private Transform _playerTransform;
    private AIRotateVehicle _aiRotateVehicle;

    public Vector3 CurrentTargetPosition
    {
        get => _targetPosition;
    }

    public float TurnSpeed
    {
        get => _turnSpeed;
    }

    private void Start()
    {
        _rb = gameObject.GetComponentInChildren<Rigidbody>();
        _aiRotateVehicle = GetComponentInChildren<AIRotateVehicle>();
        _playerTransform = Player.CurrentPlayerVehicle.transform;
        
        _model = _aiRotateVehicle.gameObject;
    }

    private void Update()
    {
        switch (_aiMode)
        {
            case AIMode.FOLLOW_PLAYER:
                FollowPlayer();
                break;
            case AIMode.FOLLOW_NODES:
                SetNode();
                break;
        }

        SetModelPosition();
        CastRays();
    }

    private void FixedUpdate()
    {
        MoveVehicle();
    }

    private void SetModelPosition() 
    {
        var newPos = new Vector3(_rb.transform.position.x, _rb.transform.position.y - _yModelOff, _rb.transform.position.z);
        _model.transform.position = newPos;
    }

    private void FollowPlayer()
    {
        if (_playerTransform == null)
            return;

        _targetPosition = _playerTransform.position;
    }

    private void SetNode() 
    {
    
    }

    private void CastRays()
    {
        float angleStep = _wallDetectAngle / (_numberOfRays - 1);  // Calculate angle between each ray

        for (int i = 0; i < _numberOfRays; i++)
        {
            float angle = _startWallDetectAngle + (i * angleStep);  // Calculate the angle for this ray
            print(angle);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * _model.transform.forward;  // Calculate the direction of the ray

            Ray ray = new Ray(_model.transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _rayDistance, _layerMask))
            {
                Debug.DrawRay(_model.transform.position, direction * hit.distance, Color.red);  // Draw the ray in red if it hits something
                _aiRotateVehicle.AdjustSteering(hit.point, angle);
                AdjustSpeed(hit.distance);
                break;
            }
            else 
            {
                Debug.DrawRay(_model.transform.position, direction * _rayDistance, Color.green);  // Draw the ray in green if it doesn't hit anything
            }
            //_aiRotateVehicle.ResetSteeringOverride();

        }
    }

    private void MoveVehicle()
    {
        var speed = _currentSpeedUsage * _moveSpeed;

        // Apply forces to simulate car movement.
        Vector3 moveForce = speed * _model.transform.forward;
        _rb.AddForce(moveForce, ForceMode.Acceleration);
    }

    private void AdjustSpeed(float distanceToWall) 
    {
        float newSpeedPerc = distanceToWall / _rayDistance;
        _currentSpeedUsage = newSpeedPerc;
    }
}
