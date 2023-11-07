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

    // Drifting Variabless
    [Space(10)]
    [Header("Drifting")]
    [SerializeField, Range(0.0f, 100f)] private float _driftIntensity = 10f;
    [SerializeField] private float _driftDecay = .95f;
    [SerializeField] private float _driftFactor = 1f;

    [Space(10)]
    [SerializeField] private float _driftTurnMultiplier = 4f;
    [SerializeField] private bool _applyDriftTurnMultiplier = false;

    // Thresholds
    [Space(10)]
    [Header("Player Movement Thresholds")]
    [SerializeField] private float _maxMoveVelocity = 50f;
    [SerializeField] private float _minSpeedToKill = 25f;

    // Engine Rigidbody -> Gets detached and is controlled while the vehicle art is set to this objects position each frame
    private Rigidbody RB;
    public Rigidbody VehicleRB { get => RB; }
    private Vector2 _moveInput;
    private LayerMask _groundMask = 10;

    private bool _grounded;
    private float _groundCheckDistance = 1f;

    private bool _isDrifting = false;
    private Vector3 _lastVeloctiy;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    // DEBUGGING
    private Vector3 testdriftForce; // used for debugging

    private void Awake()
    {
        RB = gameObject.GetComponentInChildren<Rigidbody>();

        // detach rb from parent
        RB.transform.parent = null;
    }

    private void Update()
    {
        // Set Vehicle Position to be the same as the 'Engine' Rigidbody
        transform.position = RB.position;

        // Set Player Rotation Based on Input and Whether the Player is on a ramp or not
        SetRotation();

        // Check if player hit  max velocity, if true, set to be MaxVelocity
        CheckAndSetPlayerHitMaxSpeed();

        // Set RB drag if player is falling or on the ground (Ground resistance / Air Resistance) 
        RB.drag = _grounded ? _groundDrag : _airDrag;
    }

    private void FixedUpdate()
    {
        MovePlayerIfGrounded(_isDrifting);
        _lastVeloctiy = RB.velocity;
    }

    private void MovePlayerIfGrounded(bool isDrifting)
    {
        var speed = _moveInput.y < 0 ? _moveSpeed * 0.5f : _moveSpeed;

        Player.IsMoving = CheckVelocity(0);

        // Get the input for car movement.
        float moveInput = _moveInput.y;
        float turnInput = _moveInput.x;

        // Calculate speed and torque.
        float currentSpeed = isDrifting ? speed / _driftIntensity : _moveSpeed;

        // Apply forces to simulate car movement.
        Vector3 moveForce = currentSpeed * moveInput * transform.forward;
        RB.AddForce(moveForce, ForceMode.Acceleration);

        // Apply drifting force when drifting is enabled.
        if (!isDrifting)
        {
            // Reduce drift factor over time if not drifting.
            _driftFactor = Mathf.Lerp(_driftFactor, 1f, Time.deltaTime * _driftDecay);
            return;
        }

        // Apply the player drift
        float angle = Vector3.Angle(_lastVeloctiy, transform.right);
        _driftFactor = Mathf.Lerp(_driftFactor, _driftIntensity, _driftDecay * Time.deltaTime);

        Quaternion myRotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 startingDirection = transform.right * _moveInput.x;
        Vector3 result = myRotation * startingDirection;


        Vector3 driftForce = _driftFactor * currentSpeed * (turnInput * result);
        testdriftForce = driftForce;

        RB.AddForce(driftForce);
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

    public void OnDrift(InputAction.CallbackContext context)
    {
        var isDrifting = context.ReadValue<float>();
        _isDrifting = isDrifting == 1;
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
        bool rayHitGround = Physics.Raycast(RB.transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, out hit, _groundMask);
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
        RB.velocity = RB.velocity.normalized * newSpeed;
    }

    private void Boost()
    {
        // fix boost lol
        _moveSpeed *= 2;
    }

    private bool CheckVelocity(float velocityToCompare)
    {
        var sqrMaxVelocity = velocityToCompare * velocityToCompare;
        if (RB.velocity.sqrMagnitude > sqrMaxVelocity)
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        // DEBUG
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.forward * 1000);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _lastVeloctiy * 1000);

        Debug.DrawRay(transform.position, testdriftForce * 1000, Color.red);
    }
}