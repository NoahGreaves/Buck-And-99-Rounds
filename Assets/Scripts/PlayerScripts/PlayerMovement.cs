using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Camera
    [SerializeField] private Transform _cameraFollowTarget;

    // Movement
    [Space(10)]
    [SerializeField] private float _moveForce;
    [SerializeField] private float _cameraSensitivity = 0.25f;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private float _maxMoveVelocity = 50f;
    [SerializeField] private float _minSpeedToKill = 25f;

    private Rigidbody _RB;

    private Vector2 _lookInput;
    private Vector2 _moveInput;

    private Vector3 _playerVelocity;

    private LayerMask _groundMask = 1 << 9;
    private bool _grounded;
    private float _forceMultiplier = 1f; // increase for sprint, and other speed abilities

    private float _gravity = -9.81f;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
        // TODO: MAKE SURE THIS IS THE VEHICLE RIGIDBODY
        _RB = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        MovePlayer();
        HandleRotationInput();
        _canPlayerSpeedKill = CheckPlayerCanKillWithSpeed();
    }

    private void FixedUpdate()
    {
        _grounded = IsGrounded();
    }

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

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        MovePlayer();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        _cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(_lookInput.x * _cameraSensitivity, Vector3.up);

       var eulerAngles = _cameraFollowTarget.transform.localEulerAngles;
        eulerAngles.z = 0 ;

        var xAngle = _cameraFollowTarget.transform.localEulerAngles.x;

        // clamp camera --> FIX HARD CODE VALUES
        //if (xAngle > 180 && xAngle < 240)
        //{
        //    eulerAngles.x = 340;
        //}
        //else if (xAngle < 180 && xAngle > 40)
        //{
        //    eulerAngles.x = 40;
        //}

        //_cameraFollowTarget.transform.localEulerAngles = eulerAngles;

    }

    private void MovePlayer()
    {
        // Get Player Input Movement Diretion
        var moveDirection = Vector3.zero;
        moveDirection.x = _moveInput.x;
        moveDirection.z = _moveInput.y;
        _RB.AddForce((_moveForce * _forceMultiplier) * transform.TransformDirection(moveDirection), ForceMode.Force);

        // Clamp Player Velocity to a Max Speed --->>> REPLACE WITH CHECKVELOCITY METHOD
        var atMaxVelocity = CheckVelocity(_RB.velocity, _maxMoveVelocity);
        var normalizedVelocity = _RB.velocity.normalized;
        if (atMaxVelocity)
        {
            _RB.velocity = normalizedVelocity * _maxMoveVelocity;
        }
        //var velocity = _rb.velocity;
        //var sqrMaxVelocity = _maxMoveVelocity * _maxMoveVelocity;
        //if (velocity.sqrMagnitude > sqrMaxVelocity) 
        //{
        //    _rb.velocity = velocity.normalized * _maxMoveVelocity;
        //}

        // While moving set the player rotation to be the same of the Camera follow target
        //transform.rotation = Quaternion.Euler(0, _cameraFollowTarget.transform.rotation.eulerAngles.y, 0);
        // reset the y rotation of the look transform
        //_cameraFollowTarget.localEulerAngles = new Vector3(_cameraFollowTarget.localEulerAngles.x, 0, 0);
    }

    private void Jump()
    {
        Debug.Log("player jumped: " + _grounded);
        if (_grounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpForce * 3f * _gravity);
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
