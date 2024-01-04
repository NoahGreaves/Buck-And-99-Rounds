using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleWheelRotation : MonoBehaviour
{
    [SerializeField] private float _rotationAmount = 30f;

    private Vector2 _rotationInput; 

    private void Update()
    {
        UpdateWheelRotation();
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>();
    }

    private void UpdateWheelRotation() 
    {
        // Set Vehicle Rotation only while the player is moving
        float newRot = (_rotationInput.x * _rotationAmount) * Time.deltaTime;
        //transform.Rotate(0, newRot, 0, Space.Self);

        // Get the current rotation of the object
        //float currentRotation = transform.eulerAngles.y;

        //// Clamp the rotation between the specified limits
        //float clampedRotation = Mathf.Clamp(currentRotation * newRot, -_rotationAmount, _rotationAmount);
        //transform.Rotate(0, clampedRotation, 0, Space.Self);

        // Apply the clamped rotation back to the object
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, clampedRotation, transform.eulerAngles.z);
    }
}
