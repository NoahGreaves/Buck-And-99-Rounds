using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minedropper_Pickup : Pickup
{
    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = CompareToPlayerLayer(other.gameObject.layer);
        if (!isPlayer)
            return;

        // Give player Bomb Launcher
        GameEvents.MineDropperPickup();

        // Change to Object Pooling
        Destroy(gameObject);
    }
}
