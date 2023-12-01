using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupManager : MonoBehaviour
{
    [Header("Bomb Launcher")]
    [SerializeField] private GameObject _bombLauncher;

    [Header("Forward Facing Shield")]
    [SerializeField] private GameObject _forwardShield;

    private void Start()
    {
        GameEvents.OnBombLauncherPickup += OnBombLauncherPickup;
        GameEvents.OnForwardFacingShieldPickup += OnForwardFacingShieldPickup;
    }

    private void OnDisable()
    {
        GameEvents.OnBombLauncherPickup -= OnBombLauncherPickup;
        GameEvents.OnForwardFacingShieldPickup -= OnForwardFacingShieldPickup;
    }

    private void OnBombLauncherPickup()
    {
        if (_bombLauncher == null)
            return;
        _bombLauncher.SetActive(true);
    }

    private void OnForwardFacingShieldPickup()
    {
        if (_forwardShield == null)
            return;
        _forwardShield.SetActive(true);
    }
}
