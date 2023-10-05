using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Weapon _equippedWeapon;

    // Controls
    private PlayerInputActions _playerControls;
    private InputAction _fire;

    private void Awake()
    {
        _playerControls = Player.Controls;
    }

    private void OnEnable()
    {
        _fire = _playerControls.Player.Fire;
        _fire.Enable();
        _fire.performed += Fire;
    }

    private void OnDisable()
    {
        _fire.Disable();
        _fire.performed -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        _equippedWeapon.ShootWeapon();
    }
}
