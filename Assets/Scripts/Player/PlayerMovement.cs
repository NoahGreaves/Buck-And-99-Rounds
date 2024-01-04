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
    private const float GROUND_CHECK_DISTANCE = 1f;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
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

        // Checks if player is on the ground
        _grounded = IsGrounded(out RaycastHit hit);
        Debug.DrawLine(_model.transform.position, hit.normal);
        Player.GroundNormal = hit.normal;
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
        Boost();
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

        // Check if player is moving ( Units: km/hr )
        Player.IsMoving = (int)_rb.velocity.sqrMagnitude / 3.6 > 0;
    }

    private void BoostAfterTurn()
    {
        // _boostAmount
        var isTurning = _moveInput.x != 0;
        if (!isTurning)
        {
            // boost player in forward direction after the turn is complete 
            _rb.AddForce(_boostFactor * _moveInput.y * _model.transform.forward, ForceMode.Impulse);
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
        //bool rayHitGround = Physics.Raycast(RB.transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, out hit, _groundMask);
        bool rayHitGround = Physics.Raycast(_model.transform.position, _model.transform.TransformDirection(Vector3.down) * GROUND_CHECK_DISTANCE, out hit, _groundMask);
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

    private void Boost()
    {
        // fix boost lol
        _moveSpeed *= 2;
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