using UnityEngine;

public class Fuel_Pickup : Pickup
{
    [SerializeField] private float _fuelAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = CompareToPlayerLayer(other.gameObject.layer);
        if (!isPlayer)
            return;
        GameEvents.AddPlayerFuel(_fuelAmount);

        // Change to Object Pooling
        Destroy(gameObject);
    }
}