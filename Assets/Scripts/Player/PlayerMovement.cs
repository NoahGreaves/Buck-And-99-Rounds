using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Vehicle Values
    [Header("Vehicle Movement")]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _turnSpeed = 30f;
    [SerializeField] private float _groundDrag = 4;
    [SerializeField] private float _airDrag = 0.1f;
    [SerializeField] private float _breakForce = 30f;
    [SerializeField] private float _gravityForce = -20f;

    // Thresholds
    [Space(10)]
    [Header("Player Movement Thresholds")]
    [SerializeField] private float _minSpeedToKill = 25f;
    [SerializeField] private float _maxMoveVelocity = 50f;

    // Engine Rigidbody -> Gets detached and is controlled while the vehicle art is set to this objects position each frame
    private Rigidbody _RB;
    public Rigidbody VehicleRB { get => _RB; }
    private Vector2 _moveInput;
    private LayerMask _groundMask = 10;

    private bool _isBreaking = false;
    private bool _grounded;
    private float _groundCheckDistance = 1f;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
        _RB = gameObject.GetComponentInChildren<Rigidbody>();

        // detach rb from parent
        _RB.transform.parent = null;
    }

    private void Update()
    {
        // Set Vehicle Position to be the same as the 'Engine' Rigidbody
        transform.position = _RB.position;

        // Set Player Rotation Based on Input and Whether the Player is on a ramp or not
        SetRotation();

        // Check if player hit  max velocity, if true, set to be MaxVelocity
        CheckAndSetPlayerHitMaxSpeed();

        // Set RB drag if player is falling or on the ground (Ground resistance / Air Resistance) 
        _RB.drag = _grounded ? _groundDrag : _airDrag;
    }

    private void FixedUpdate()
    {
        if (_grounded)
        {
            // If player is reversing half the player moveSpeed
            var speed = _moveInput.y < 0 ? _moveSpeed * 0.5f : _moveSpeed;
            _RB.AddForce((transform.forward * _moveInput.y) * speed, ForceMode.Acceleration);
        }   
        else
            _RB.AddForce(-transform.up * _gravityForce);

    }

    // Get Player Input
    #region Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        Boost();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        var isBreaking = context.ReadValue<float>();
        _isBreaking = isBreaking == 1;

        // DELETE 
        SetPlayerSpeed(0f);
    }
    #endregion

    private void SetRotation()
    {
        var isVehicleMoving = CheckVelocity(0);

        // Set Vehicle Rotation only while the player is moving
        float newRot = isVehicleMoving ? (_moveInput.x * _turnSpeed) * Time.deltaTime : 0f;
        transform.Rotate(0, newRot, 0, Space.World);

        CheckPlayerCanKillWithSpeed();

        _grounded = IsGrounded(out RaycastHit hit);
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    private bool IsGrounded(out RaycastHit hit)
    {
        bool rayHitGround = Physics.Raycast(_RB.transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, out hit, _groundMask);
        if (rayHitGround)
            return true;
        else
            return false;
    }

    private void CheckPlayerCanKillWithSpeed()
    {
        bool canPlayerSpeedKill = CheckVelocity(_minSpeedToKill);
        Player.CanSpeedKill = canPlayerSpeedKill;
    }

    private void CheckAndSetPlayerHitMaxSpeed()
    {
        bool playerMovingMaxSpeed = CheckVelocity(_maxMoveVelocity);
        if (!playerMovingMaxSpeed)
            return;

        SetPlayerSpeed(_maxMoveVelocity);
    }

    private void SetPlayerSpeed(float newSpeed)
    {
        _RB.velocity = _RB.velocity.normalized * newSpeed;
    }

    private void Boost()
    {
        // fix boost lol
        _moveSpeed *= 2;
    }

    private bool CheckVelocity(float velocityToCompare)
    {
        var sqrMaxVelocity = velocityToCompare * velocityToCompare;
        if (_RB.velocity.sqrMagnitude > sqrMaxVelocity)
            return true;
        return false;
    }
}