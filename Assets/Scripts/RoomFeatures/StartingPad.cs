using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPad : MonoBehaviour
{
    [SerializeField] private float _timer = 3f;

    private float _currentTimer;
    private bool _startTimer = false;

    private void Start()
    {
        _currentTimer = _timer;
        OnRoomLoad();
    }

    private void OnRoomLoad()
    {
        Player.CanMove = false;
        SetPlayerPosition();

        _startTimer = true;
        StartCoroutine(StartTimer());
    }

    private void SetPlayerPosition()
    {
        Player.CurrentPlayerVehicle.SetVelocity(Vector3.zero);
        Player.CurrentPlayerVehicle.SetPosition(transform.position);
    }

    private IEnumerator StartTimer() 
    {
        while (_startTimer) 
        {
            if (_currentTimer < 0)
                break;

            _currentTimer -= Time.deltaTime;
            Player.CurrentPlayerVehicle.SetVelocity(Vector3.zero);
            var time = Mathf.CeilToInt(_currentTimer);
            print(time);

            yield return null;
        }

        _currentTimer = _timer;
        _startTimer = false;
        Player.CanMove = true;
        StopCoroutine(StartTimer());
        yield return null;
    }
}
