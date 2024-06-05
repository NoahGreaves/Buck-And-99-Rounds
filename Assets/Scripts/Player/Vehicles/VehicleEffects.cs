using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleEffects : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] _tireMarks;

    private bool _tireMarksFlag;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Controller_Steer.performed += StartTireMarkEmitter;
        _playerInputActions.Player.Controller_Steer.canceled += StopTireMarkEmitter;
        _playerInputActions.Player.Controller_Steer.Enable();

    }

    private void OnDisable()
    {
        _playerInputActions.Player.Controller_Steer.performed -= StartTireMarkEmitter;
        _playerInputActions.Player.Controller_Steer.canceled -= StopTireMarkEmitter;
        _playerInputActions.Player.Controller_Steer.Disable();
    }

    // when player brakes or reverses, set brake lights. once moving in Negative direction, set lights to white
    private void SetBrakeLights(bool isOn) 
    { }

    private void SetHeadLights(bool isOn) { }

    private void StartTireMarkEmitter(InputAction.CallbackContext context)
    {
        //print(_tireMarksFlag + " " + Player.IsGrounded);
        if (!Player.IsGrounded )
        {
            return;
        }

        for (int i = 0; i < _tireMarks.Length; i++)
        {
            _tireMarks[i].emitting = true;
        }

        _tireMarksFlag = true;
    }
    
    private void StopTireMarkEmitter(InputAction.CallbackContext context)
    {
        if (!_tireMarksFlag)
            return;

        for (int i = 0; i < _tireMarks.Length; i++)
        {
            _tireMarks[i].emitting = false;
        }

        _tireMarksFlag = false;
    }
}
