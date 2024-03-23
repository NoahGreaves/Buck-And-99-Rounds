using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Vehicle Values
    [Header("Vehicle")]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _maxSpeed = 200f;

    [Header("Boost")]
    [SerializeField] private float _boostAmount = 5f;
    [SerializeField] private float _boostIncrement = 0.95f;
    [SerializeField] private float _boostFactor = 1f;

    [Header("Gravity")]
    [SerializeField] private float _fallModifier = 2.5f;

    // Thresholds
    [Space(10)]
    [Header("Player Movement Thresholds")]
    [SerializeField] private float _minSpeedToKill = 25f;

    // Engine Rigidbody -> Gets detached and is controlled while the vehicle art is set to this objects position each frame
    private Rigidbody _rb;
    private Vector2 _moveInput;
    private LayerMask _groundMask = 10;
    private Fuel _playerFuel;

    private bool _grounded;
    [SerializeField] private float GROUND_CHECK_DISTANCE = 1f;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
        Player.Model = _model;
        Player.TotalBoostAmount = _boostAmount;
        _rb = gameObject.GetComponentInChildren<Rigidbody>();
        _playerFuel = GetComponent<Fuel>();

        // detach rb from parent
        // RB.transform.parent = null;
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
            _playerFuel.enabled = !_playerFuel.enabled;
        }

        if (_rb.velocity.y < 0)
        {
            _rb.AddForce(Physics.gravity * (_fallModifier) * Time.deltaTime);
        }

        // Checks if player is on the ground
        _grounded = IsGrounded(out RaycastHit hit);
        Debug.DrawLine(_model.transform.position, _model.transform.up * (-GROUND_CHECK_DISTANCE), Color.green);
        //Player.GroundNormal = hit.normal;
        Player.IsGrounded = _grounded;

        if (_grounded)
        {
            //var target = new Vector3(hit.normal.x, _model.transform.rotation.y, hit.normal.z);
            //_model.transform.up = Vector3.Lerp(_model.transform.up, target, Time.deltaTime * 1f);
            //_model.transform.Rotate(0, _model.transform.eulerAngles.y, 0);

            //var rotation = _grounded ? hit.normal : _model.transform.rotation.eulerAngles;
            //var rotation = hit.normal;
            //_model.transform.rotation = Quaternion.Euler(rotation.x, _model.transform.rotation.y, rotation.z);
        }
    }

    // Get Player Input
    #region Player Input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_grounded || !Player.CanMove)
        {
            return;
        }

        _moveInput = context.ReadValue<Vector2>();
        SetFuelUsage();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        //Boost();
    }

    #endregion

    private void MovePlayer()
    {
        var speed = _moveInput.y < 0 ? _moveSpeed * 0.5f : _moveSpeed;

        // Get the input for car movement.
        float moveInput = _moveInput.y;

        // Apply forces to simulate car movement.
        Vector3 moveForce = speed * moveInput * _model.transform.forward;
        _rb.AddForce(moveForce, ForceMode.Acceleration);

        if (_rb.velocity.sqrMagnitude > _maxSpeed)
            _rb.velocity *= 0.99f;

        // Check if player is moving
        Player.IsMoving = _rb.velocity.sqrMagnitude > 0f;
    }

    private void BoostAfterTurn()
    {
        // _boostAmount
        var isTurning = _moveInput.x != 0;
        if (!isTurning)
        {
            // boost player in forward direction after the turn is complete 

            var boostAmount = _boostFactor * _moveInput.y * _model.transform.forward;
            _rb.AddForce(boostAmount, ForceMode.Impulse);
            _boostFactor = 0f;
            GameEvents.PlayerBoostChange(_boostFactor);
        }
        if (isTurning)
        {
            _boostFactor = Mathf.Lerp(_boostFactor, _boostAmount, _boostIncrement * Time.deltaTime);
            GameEvents.PlayerBoostChange(_boostFactor);
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
        bool rayHitGround = Physics.Raycast(_model.transform.position, _model.transform.TransformDirection(Vector3.down) * GROUND_CHECK_DISTANCE, out hit, _groundMask);
        if (rayHitGround)
            return true;
        else
            return false;
        //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1, (int)_groundMask);
        //distToGround = GetComponent<Collider>().bounds.extents.y;
        //return Physics.Raycast(transform.position, -Vector3.up, GROUND_CHECK_DISTANCE, (int)_groundMask);
    }

    private void CheckPlayerCanKillWithSpeed()
    {
        bool canPlayerSpeedKill = CheckVelocity(_minSpeedToKill);
        Player.CanSpeedKill = canPlayerSpeedKill;
    }

    private bool CheckVelocity(float velocityToCompare)
    {
        var sqrMaxVelocity = velocityToCompare * velocityToCompare;
        if (_rb.velocity.sqrMagnitude > sqrMaxVelocity)
            return true;
        return false;
    }

    public void SetPlayerPosition(Vector3 newPosition)
    {
        _rb.position = newPosition;
    }
}