using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForwardFacingShield : MonoBehaviour
{
    [SerializeField] private float _upTime = 1f;
    [SerializeField] private float _cooldown = 1.5f;
    //[SerializeField] private float _regenAmount = .5f;

    private bool _useShield = false;
    private bool _isOnCooldown = false;
    private float _currentUpTime;
    private float _currentCooldown;

    private Collider _collider;
    private Renderer _renderer;

    private void OnEnable()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _currentUpTime = _upTime;
        SetShield(false);
    }

    private void Update()
    {
        if (_useShield && !_isOnCooldown)
            SetShield(true);
        else
            SetShield(false);

        // make regeneritive
        _currentUpTime = _useShield && !_isOnCooldown ? _currentUpTime - Time.deltaTime : _currentUpTime;
        if (_currentUpTime <= 0)
        {
            _isOnCooldown = true;
            _currentUpTime = _upTime;
            SetShield(false);
        }

        _currentCooldown = _isOnCooldown ? _currentCooldown - Time.deltaTime : _currentCooldown;
        if (_currentCooldown <= 0)
        {
            _isOnCooldown = false;
            _currentCooldown = _cooldown;
        }
    }

    public void OnUseAbility(InputAction.CallbackContext context)
    {
        var useShield = context.ReadValue<float>();
        _useShield = useShield == 1;
    }

    private void SetShield(bool enabled)
    {
        _collider.enabled = enabled;
        _renderer.enabled = enabled;
    }
}
