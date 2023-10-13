using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private Vector2 _lookInput;
    private Vector2 _moveInput;

    private Vector3 _playerVelocity;

    // Movement
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private float _jumpForce = 10;
    private bool _grounded;
    private float _moveSpeed;
    private float _moveSpeedMultiplier = 1f; // increase for sprint, and other speed abilities

    private float _gravity = -9.81f;

    // Camera
    [SerializeField] private Transform _cameraFollowTarget;

    // Controls
    private CharacterController _controller;
    private PlayerInputActions _playerControls;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _look;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _playerControls = Player.Controls;
        _moveSpeed = Player.MovementSpeed;
    }

    private void Update()
    {
        _grounded = _controller.isGrounded;

        MovePlayer();
        HandleRotationInput();
    }

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
    }

    private void HandleRotationInput()
    {
        _cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(_lookInput.x * _rotationSpeed, Vector3.up);

        //_cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(_lookInput.y * _rotationSpeed, Vector3.right);

        var angles = _cameraFollowTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = _cameraFollowTarget.transform.localEulerAngles.x;

        if (angle > 180 && angle < 240)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        _cameraFollowTarget.transform.localEulerAngles = angles;

    }

    private void MovePlayer()
    {
        Debug.Log("zzz");
        var moveDirection = Vector3.zero;
        moveDirection.x = _moveInput.x;
        moveDirection.z = _moveInput.y;
        _controller.Move((_moveSpeed * _moveSpeedMultiplier) * Time.deltaTime * transform.TransformDirection(moveDirection));

        _playerVelocity.y += _gravity * Time.deltaTime;
        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _controller.Move(_playerVelocity * Time.deltaTime);

        { /// Make this happen only when the player is moving
            // While moving set the player rotation to be the same of the Camera follow target
            transform.rotation = Quaternion.Euler(0, _cameraFollowTarget.transform.rotation.eulerAngles.y, 0);
            //// reset the y rotation of the look transform
            _cameraFollowTarget.localEulerAngles = new Vector3(_cameraFollowTarget.localEulerAngles.x, 0, 0);
        }
    }

    private void Jump()
    {
        Debug.Log("player jumped: " + _grounded);
        if (_grounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpForce * 3f * _gravity);
        }
    }
}
