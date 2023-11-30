using UnityEngine;
using UnityEngine.InputSystem;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] private Explosive _explosivePrefab;
    [SerializeField] private Transform _explosiveSpawn;
    [SerializeField] private float _firepower = 2f;
    [SerializeField] private float _cooldown = 0.4f;

    private bool _isFiring = false;
    private float _maxCooldown;

    private void Start()
    {
        _maxCooldown = _cooldown;
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        var isFiring = context.ReadValue<float>();
        _isFiring = isFiring == 1;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_isFiring && _cooldown <= 0)
        {
            FireBomb();
            _cooldown = _maxCooldown;
        }
    }

    private void FireBomb()
    {
        var explosive = Instantiate(_explosivePrefab);
        explosive.transform.parent = null;
        explosive.transform.forward = _explosiveSpawn.forward;
        explosive.transform.SetPositionAndRotation(_explosiveSpawn.position, _explosiveSpawn.rotation);

        var rb = explosive.GetComponent<Rigidbody>();
        rb.AddForce(_explosiveSpawn.forward * _firepower, ForceMode.Impulse);
    }
}
