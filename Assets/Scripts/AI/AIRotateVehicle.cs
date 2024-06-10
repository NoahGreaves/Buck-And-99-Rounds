using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRotateVehicle : MonoBehaviour
{
    private AIVehicleController _aiVehicleController;
    private float _rotationMargin = 1f;

    private bool _steeringOverride;

    private void Awake()
    {
        _aiVehicleController = GetComponentInParent<AIVehicleController>();
    }

    private void Update()
    {
        if (!_steeringOverride)
           RotateVehicle();
    }

    private void RotateVehicle()
    {
        Vector3 targetDirection = _aiVehicleController.CurrentTargetPosition - transform.position;
        targetDirection.y = 0;  // Keep the object upright (optional, depending on your game)

        // Calculate the rotation needed to face the target
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        float turnSpeed = _aiVehicleController.TurnSpeed * Time.deltaTime;

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
    }

    public void ResetSteeringOverride() => _steeringOverride = false;

    public void AdjustSteering(Vector3 rayHitPoint, float angleToWall)
    {
        _steeringOverride = true;

        print(angleToWall);

        Vector3 wallDirection = transform.position - rayHitPoint;
        if (wallDirection.magnitude < _rotationMargin) { return; }

        // Get Rotation from current rotation to wall direction 
        Quaternion awayRotation = Quaternion.LookRotation(wallDirection, transform.up);
        Vector3 euler = awayRotation.eulerAngles;
        euler.y += angleToWall; // add the angle from WallDirection to Forward vector to the current y-axis rotation 
        awayRotation = Quaternion.Euler(euler);

        // Rotate the car
        float turnSpeed = (_aiVehicleController.TurnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, awayRotation, turnSpeed);
    }
}
