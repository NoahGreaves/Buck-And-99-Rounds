using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStartTimer : MonoBehaviour
{
    [SerializeField] private float _timer = 3f;
    [SerializeField] private UIController _uiController;

    private float _currentTimer;
    private bool _startTimer = false;

    private void Awake()
    {
        _currentTimer = _timer;

        GameEvents.OnRoomLoad += OnRoomLoad;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomLoad -= OnRoomLoad;
    }

    private void OnRoomLoad()
    {
        _startTimer = true;
        Player.CanMove = false;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        _uiController.SetCountdownEnabled(true);
        while (_startTimer)
        {
            if (_currentTimer < 0)
                break;

            _currentTimer -= Time.deltaTime;
            Player.CurrentPlayerVehicle.SetVelocity(Vector3.zero);
            var time = Mathf.CeilToInt(_currentTimer);
            GameEvents.CountdownUpdate(time);

            yield return null;
        }

        _currentTimer = _timer;
        _startTimer = false;
        Player.CanMove = true;
        _uiController.SetCountdownEnabled(false);
        StopCoroutine(StartTimer());
        yield return null;
    }
}
