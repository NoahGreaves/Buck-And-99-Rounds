using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleModelPosition : MonoBehaviour
{
    [SerializeField] private GameObject _playerCar;
    [SerializeField] private float _turnSpeed = 30f;

    private Vector2 _rotationInput;

    private void Start()
    {
        transform.parent = null;
    }

    private void Update()
    {
        SetRotation();

        transform.position = _playerCar.transform.position;
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>();
    }

    private void SetRotation()
    {
        // Set Vehicle Rotation only while the player is moving
        float newRot = Player.IsMoving ? (_rotationInput.x * _turnSpeed) * Time.deltaTime : 0f;
        transform.Rotate(0, newRot, 0, Space.World);

    }
}
