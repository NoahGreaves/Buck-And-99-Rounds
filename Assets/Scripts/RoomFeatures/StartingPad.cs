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
        Player.CurrentPlayerVehicle.SetPosition(transform.position);
    }
}
