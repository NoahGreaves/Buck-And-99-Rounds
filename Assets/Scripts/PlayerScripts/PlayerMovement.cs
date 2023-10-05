using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private Vector2 _lookValue;

    // Movement
    private float _moveSpeed = 100;
    //private bool _isJumping = false;

    // Constorls
    private PlayerInputActions _playerControls;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _look;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _playerControls = Player.Controls;
        _moveSpeed = Player.MovementSpeed;
    }

    private void OnEnable()
    {
        GetControls();
        EnableControls();
        SubscribeEvents();
    }

    private void OnDisable()
    {
        DisableControls();
        UnsubscribeEvents();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_moveDirection.x * _moveSpeed * Time.deltaTime, 0, _moveDirection.y * _moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        HandleRotationInput();
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void HandleRotationInput() 
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
        }
    }

    private void Look(InputAction.CallbackContext context)
    {
        _lookValue = context.ReadValue<Vector2>();
    }

    private void GetControls()
    {
        _move = _playerControls.Player.Move;
        _jump = _playerControls.Player.Jump;
        _look = _playerControls.Player.Look;
    }

    private void EnableControls()
    {
        _move.Enable();
        _look.Enable();
    }

    private void SubscribeEvents()
    {
        _look.performed += Look;
    }

    private void DisableControls()
    {
        _move.Disable();
        _look.Disable();
    }

    private void UnsubscribeEvents()
    {
        _look.performed -= Look;
    }
}
