using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool _enableBoostAfterTurn = true;

    // Vehicle Values
    [Header("Vehicle")]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _maxSpeed = 200f;
    [SerializeField] private float _turnSpeedMultiplier = 1.5f;

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

    private PlayerInputActions _playerInputActions;

    // Engine Rigidbody -> Gets detached and is controlled while the vehicle art is set to this objects position each frame
    private Rigidbody _rb;
    private Vector2 _moveInput;
    private Fuel _playerFuel;

    private bool _canPlayerSpeedKill = false;
    public bool CanPlayerSpeedKill { get => _canPlayerSpeedKill; }

    private void Awake()
    {
        Player.Model = _model;
        Player.TotalBoostAmount = _boostAmount;

        _playerInputActions = new PlayerInputActions();

        _rb = gameObject.GetComponentInChildren<Rigidbody>();
        _playerFuel = GetComponent<Fuel>();

        // detach rb from parent
        // RB.transform.parent = null;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Controller_Move_Forward.performed += ControllerOnMove;
        _playerInputActions.Player.Controller_Move_Forward.canceled += ControllerOnMoveCanceled;
        _playerInputActions.Player.Controller_Move_Forward.Enable();
        
        _playerInputActions.Player.Controller_Move_Backwards.performed += ControllerOnReverse;
        _playerInputActions.Player.Controller_Move_Backwards.canceled += ControllerOnMoveCanceled;
        _playerInputActions.Player.Controller_Move_Backwards.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Controller_Move_Forward.performed -= ControllerOnMove;
        _playerInputActions.Player.Controller_Move_Forward.canceled -= ControllerOnMoveCanceled;
        _playerInputActions.Player.Controller_Move_Forward.Disable();

        _playerInputActions.Player.Controller_Move_Backwards.performed -= ControllerOnReverse;
        _playerInputActions.Player.Controller_Move_Backwards.canceled -= ControllerOnMoveCanceled;
        _playerInputActions.Player.Controller_Move_Backwards.Disable();
    }

    private void Update()
    {
        // Check if the player is moving fast enough to eliminate an enemy
        CheckPlayerCanKillWithSpeed();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (_enableBoostAfterTurn)
            BoostAfterTurn();

        // REMOVE AND MAKE THIS PART OF A DEBUG MENU 
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _playerFuel.enabled = !_playerFuel.enabled;
        }

        if (_rb.velocity.y < 0)
        {
            _rb.AddForce((_fallModifier) * Time.deltaTime * Physics.gravity);
        }

        if (Player.IsGrounded)
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
        if (!Player.IsGrounded || !Player.CanMove)
        {
            return;
        }

       _moveInput = context.ReadValue<Vector2>();

        SetFuelUsage();
    }

    public void ControllerOnMove(InputAction.CallbackContext context)
    {
        _moveInput = new Vector2(0f, 1f);
    }

    public void ControllerOnReverse(InputAction.CallbackContext context) 
    {
        _moveInput = new Vector2(0f, -1f);
    }

    public void ControllerOnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = new Vector2(0f, 0f);    
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        //Boost();
    }

    #endregion

    private void MovePlayer()
    {
        var speed = _moveInput.y < 0 ? _moveSpeed * 0.5f : _moveSpeed;
        
        // Get the forward movement input for car 
        float moveForwardInput = _moveInput.y;

        // Apply forces to simulate car movement.
        Vector3 moveForce = speed * moveForwardInput * _model.transform.forward;
        _rb.AddForce(moveForce, ForceMode.Acceleration);

        if (_rb.velocity.sqrMagnitude > _maxSpeed)
            _rb.velocity *= 0.99f;


        if (Mathf.Abs(_moveInput.x) > 0 && _moveInput.y != 0)
        {
            Vector3 turnSpeed = _model.transform.forward * speed * _turnSpeedMultiplier * Mathf.Abs(_moveInput.x);
            _rb.AddForce(turnSpeed);
        }

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

        _boostFactor = Mathf.Lerp(_boostFactor, _boostAmount, _boostIncrement * Time.deltaTime);
        GameEvents.PlayerBoostChange(_boostFactor);
    }

    private void SetFuelUsage()
    {
        if (_moveInput.y != 0)
            _playerFuel.SetUseFuel(true);
        else
            _playerFuel.SetUseFuel(false);
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