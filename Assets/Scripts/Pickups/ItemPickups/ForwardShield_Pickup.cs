using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShield_Pickup : Pickup
{
    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = CompareToPlayerLayer(other.gameObject.layer);
        if (!isPlayer)
            return;

        // Give player Forward Facing Shield
        GameEvents.ForwardFacingShieldPickup();

        // Change to Object Pooling
        Destroy(gameObject);
    }
}
