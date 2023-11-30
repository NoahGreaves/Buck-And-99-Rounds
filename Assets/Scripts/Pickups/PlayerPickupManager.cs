using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupManager : MonoBehaviour
{
    [Header("Bomb Launcher")]
    [SerializeField] private GameObject _bombLauncher;

    private void Start()
    {
        GameEvents.OnBombLauncherPickup += OnBombLauncherPickup;
    }

    private void OnDisable()
    {
        GameEvents.OnBombLauncherPickup -= OnBombLauncherPickup;
    }

    private void OnBombLauncherPickup()
    {
        if (_bombLauncher == null)
            return;
        _bombLauncher.SetActive(true);
    }
}
