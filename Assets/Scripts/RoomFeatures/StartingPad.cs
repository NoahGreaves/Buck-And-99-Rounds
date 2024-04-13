using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPad : MonoBehaviour
{
    private void Start()
    {
        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        Player.CurrentPlayerVehicle.SetVelocity(Vector3.zero);
        Player.CurrentPlayerVehicle.SetPositionAndRotation(transform.position, transform.forward);

        // Update UI so Lap Counter is set to 1 at the start of each room
        GameEvents.LapCountUpdate(0);
    }
}
