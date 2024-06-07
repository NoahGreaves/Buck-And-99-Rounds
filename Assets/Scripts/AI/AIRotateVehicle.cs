using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRotateVehicle : MonoBehaviour
{
    private AIVehicleController _aiVehicleController;

    private void Awake()
    {
        _aiVehicleController = GetComponentInParent<AIVehicleController>();
    }

    private void Update()
    {
        RotateVehicle();
    }

    private void RotateVehicle()
    {
        Vector3 targetDirection = _aiVehicleController.CurrentTargetPosition - transform.position;
        targetDirection.y = 0;  // Keep the object upright (optional, depending on your game)

        // Calculate the rotation needed to face the target
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _aiVehicleController.TurnSpeed * Time.deltaTime);
    }
}
