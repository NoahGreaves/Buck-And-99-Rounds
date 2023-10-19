using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Weapon _equippedWeapon;

    public void Fire(InputAction.CallbackContext context)
    {
        _equippedWeapon.ShootWeapon();
    }
}
