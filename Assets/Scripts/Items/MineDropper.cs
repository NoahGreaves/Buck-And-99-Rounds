using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MineDropper : MonoBehaviour
{
    [SerializeField] private Mine _minePrefab;
    [SerializeField] private float _cooldown;

    private bool _hasFired = false;
    private float _maxCooldown;

    private void Start()
    {
        _maxCooldown = _cooldown;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_hasFired && _cooldown <= 0)
        {
            DropMine();
            ResetCooldown();
        }
    }

    public void OnAltFire(InputAction.CallbackContext context)
    {
        var hasDroppedMine = context.ReadValue<float>();
        _hasFired = hasDroppedMine == 1;
    }

    private void DropMine() 
    {
        print("[Mine Dropper] Mine Dropped");
        Instantiate(_minePrefab, transform.position, _minePrefab.transform.rotation);
    }

    private void ResetCooldown() => _cooldown = _maxCooldown;
}
