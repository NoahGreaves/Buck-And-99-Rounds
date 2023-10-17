using UnityEngine;
using UnityEngine.InputSystem;

public enum WheelDriveType
{
    FRONT_WHEEL = 0,
    REAR_WHEEL,
    ALL_WHEEL
}

public class PlayerMovement : MonoBehaviour
{
    // Camera
    [Header("Camera")]
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private float _cameraSensitivity = 0.25f;

    // Vehicle Values
    [Space(10)]
    [Header("Vehicle Movement")]
    [SerializeField] private WheelDriveType _wheelDriveType = WheelDriveType.FRONT_WHEEL;
    [SerializeField] private float _motorForce = 1000f;
    [SerializeField] private float _breakForce = 3000f;
    [SerializeField] private float _maxSteerAngle = 30f;

    // Thresholds
    [Space(10)]
    [Header("Player Movement Thresholds")]
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private float _maxMoveVelocity = 50f;
    [SerializeField] private float _minSpeedToKill = 25f;

    // Wheels
    [Space(10)]
    [Header("Wheel Visuals")]
    [SerializeField] private Transform _frontLeftWheelVisual;
    [SerializeField] private Transform _frontRightWheelVisual;
    [SerializeField] private Transform _rearLeftWheelVisual;
    [SerializeField] private Transform _rearRightWheelVisual;

    [Space(10)]
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider _frontLeftWheel;
    [SerializeField] private WheelCollider _frontRightWheel;
    [SerializeField] private WheelCollider _rearLeftWheel;
    [SerializeField] private WheelCollider _rearRightWheel;

    private Rigidbody _RB;

    private Vector2 _lookInput;
    private Vector2 _moveInput;

    private Vector3 _playerVelocity;

    private LayerMask _groundMask = 1 << 9;
    private bool _grounded;
    private float _forceMultiplier = 1f; // increase for sprint, and other speed abilities

    private float _gravity = -9.81f;
    private bool _isBreaking = false;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
        // TODO: MAKE SURE THIS IS THE VEHICLE RIGIDBODY
        _RB = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        CameraRotation();

        _canPlayerSpeedKill = CheckPlayerCanKillWithSpeed();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        //UpdateWheels();

        _grounded = IsGrounded();
    }

    // Get Player Input
    #region Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
        CameraRotation();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        var isBreaking = context.ReadValue<float>();
        _isBreaking = isBreaking == 1;
    }
    #endregion

    private bool IsGrounded()
    {
        bool rayHitGround = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundCheckDistance, _groundMask);
        if (rayHitGround)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, Color.white);
            return false;
        }
    }

    private bool CheckPlayerCanKillWithSpeed()
    {
        bool canPlayerSpeedKill = CheckVelocity(_RB.velocity, _minSpeedToKill);
        if (canPlayerSpeedKill)
            return true;
        return false;
    }

    private bool CheckPlayerHitMaxSpeed()
    {
        bool playerMovingMaxSpeed = CheckVelocity(_RB.velocity, _maxMoveVelocity);
        if (playerMovingMaxSpeed)
            return true;
        return false;
    }

    private void CameraRotation()
    {
        _cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(_lookInput.x * _cameraSensitivity, Vector3.up);
    }

    private void HandleMotor()
    {
        switch (_wheelDriveType)
        {
            case WheelDriveType.FRONT_WHEEL:
                _frontLeftWheel.motorTorque  = _moveInput.y * _motorForce;
                _frontRightWheel.motorTorque = _moveInput.y * _motorForce;
                break;
            case WheelDriveType.REAR_WHEEL:
                _rearLeftWheel.motorTorque  = _moveInput.x * _motorForce;
                _rearRightWheel.motorTorque = _moveInput.x * _motorForce;
                break;
            case WheelDriveType.ALL_WHEEL:
                _frontLeftWheel.motorTorque  = _moveInput.y * _motorForce;
                _frontRightWheel.motorTorque = _moveInput.y * _motorForce;
                _rearLeftWheel.motorTorque   = _moveInput.x * _motorForce;
                _rearRightWheel.motorTorque  = _moveInput.x * _motorForce;
                break;
        }

        var currentbreakForce = _isBreaking ? _breakForce : 0f;
        ApplyBreaking(currentbreakForce);
    }

    private void ApplyBreaking(float breakForce)
    {
        _frontLeftWheel.brakeTorque = breakForce;
        _frontRightWheel.brakeTorque = breakForce;

        _rearLeftWheel.brakeTorque = breakForce;
        _rearRightWheel.brakeTorque = breakForce;
    }

    private void HandleSteering()
    {
        var currentSteerAngle = _maxSteerAngle * _moveInput.x;
        _frontLeftWheel.steerAngle = currentSteerAngle;
        _frontRightWheel.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(_frontLeftWheel, _frontLeftWheelVisual);
        UpdateSingleWheel(_frontRightWheel, _frontRightWheelVisual);
        UpdateSingleWheel(_rearLeftWheel, _rearLeftWheelVisual);
        UpdateSingleWheel(_rearRightWheel, _rearRightWheelVisual);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void Jump()
    {
        Debug.Log("player jumped: " + _grounded);
        if (_grounded)
        {
            //_playerVelocity.y = Mathf.Sqrt(_jumpForce * 3f * _gravity);
        }
    }

    private bool CheckVelocity(Vector3 playerVelocity, float velocityToCompare)
    {
        // Clamp Player Velocity to a Max Speed
        var rbVelocity = playerVelocity;
        var sqrMaxVelocity = velocityToCompare * velocityToCompare;
        if (rbVelocity.sqrMagnitude > sqrMaxVelocity)
            return true;

        return false;
    }
}
