using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Vehicle Values
    [Header("Vehicle")]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _moveSpeed = 50f;

    [Header("Boost")]
    [SerializeField] private float _boostIntesity = 5f;
    [SerializeField] private float _boostIncrement = 0.95f;
    [SerializeField] private float _boostFactor = 1f;

    // Drifting Variabless
    [Space(10)]
    [Header("Drifting")]
    [SerializeField] private float _driftIntensity = 5;
    [SerializeField] private float _driftDecay = .95f;
    [SerializeField] private float _driftFactor = 1f;
    [SerializeField] private float _driftBoostFactor = 2f;

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
    private Fuel _playerFuel;

    private bool _grounded;
    private float _groundCheckDistance = 1f;

    private bool _isDrifting = false;
    private Vector3 _lastVeloctiy;
    [SerializeField] private float _driftBoostAmount = 0f;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    // DEBUGGING
    private Vector3 testdriftForce; // used for debugging

    private void Awake()
    {
        RB = gameObject.GetComponentInChildren<Rigidbody>();
        _playerFuel = GetComponent<Fuel>();

        // detach rb from parent
        RB.transform.parent = null;
    }

    private void Update()
    {
        // Check if the player is moving fast enough to eliminate an enemy
        CheckPlayerCanKillWithSpeed();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        BoostAfterTurn();

        // REMOVE AND MAKE THIS PART OF A DEBUG MENU 
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _playerFuel.enabled = !_playerFuel;
        }
        //_grounded = IsGrounded(out RaycastHit hit);
    }

    // Get Player Input
    #region Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        SetFuelUsage();
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

    private void MovePlayer()
    {
        var speed = _moveInput.y < 0 ? _moveSpeed * 0.5f : _moveSpeed;

        Player.IsMoving = CheckVelocity(0);

        // Get the input for car movement.
        float moveInput = _moveInput.y;

        // Apply forces to simulate car movement.
        Vector3 moveForce = speed * moveInput * _model.transform.forward;
        RB.AddForce(moveForce, ForceMode.Acceleration);

        //Drift(currentSpeed);
    }

    // SWITCH TO HANDBRAKE THAT SLOWS YOU OVER TIME IN THE CURRENT DIRECTION THE PLAYER IS MOVING
    private void Drift(float currentSpeed)
    {
        float turnInput = _moveInput.x;

        // Apply drifting force when drifting is enabled.
        if (!_isDrifting)
        {
            // Reduce drift factor over time if not drifting.
            _driftFactor = Mathf.Lerp(_driftFactor, 1f, Time.deltaTime * _driftDecay);

            // if player has 
            if (_driftBoostAmount > 0)
            {
                RB.AddForce(transform.forward * _driftBoostAmount, ForceMode.Impulse);
                _driftBoostAmount = 0f;
            }

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

        _driftBoostAmount += _driftBoostFactor * Time.deltaTime;
        //RB.AddForce(driftForce, ForceMode.Acceleration);
    }

    private void BoostAfterTurn()
    {
        // _boostAmount
        var isTurning = _moveInput.x != 0;
        if (!isTurning)
        {
            // boost player in forward direction after the turn is complete 
            RB.AddForce(_boostFactor * _moveInput.y * _model.transform.forward, ForceMode.Impulse);
            _boostFactor = 0f;
        }
        if (isTurning)
        {
            _boostFactor = Mathf.Lerp(_boostFactor, _boostIntesity, _boostIncrement * Time.deltaTime);

        }
    }

    private void SetFuelUsage()
    {
        if (_moveInput.y != 0)
            _playerFuel.SetUseFuel(true);
        else
            _playerFuel.SetUseFuel(false);
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
        Gizmos.DrawLine(_model.transform.position, _model.transform.forward * 1000);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_model.transform.position, _lastVeloctiy * 1000);

        Debug.DrawRay(_model.transform.position, testdriftForce * 1000, Color.red);
    }
}