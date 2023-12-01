using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardFacingShield : MonoBehaviour
{
    [SerializeField] private float _shieldUptown;
    [SerializeField] private float _upTime = 3f;

    private bool _isUp = false;
    private float _currentUpTime;

    private void Update()
    {
        _currentUpTime = _upTime;
    }
}
