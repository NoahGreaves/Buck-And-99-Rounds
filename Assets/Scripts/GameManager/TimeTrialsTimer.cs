using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialsTimer : MonoBehaviour
{
    [SerializeField] private UIController uiController;

    private bool _countTime = false;

    private float _currentTime = 0f;
    private float _finalTime;
    
    private void Start()
    {
        GameEvents.OnTimeTrialTimerStart += StartTimer;
        GameEvents.OnTimeTrialTimerEnd += EndTimer;

        // Set initial time 00:00:00
        uiController.UpdateTimeTrialTimer(_currentTime);
    }

    private void OnDisable()
    {
        GameEvents.OnTimeTrialTimerStart -= StartTimer;   
        GameEvents.OnTimeTrialTimerEnd -= EndTimer;   
    }

    private void Update()
    {
        if (!_countTime)
            return;

        _currentTime += Time.deltaTime;
        uiController.UpdateTimeTrialTimer(_currentTime);
    }

    private void StartTimer() 
    {
        _countTime = true;
    }

    private void EndTimer() 
    {
        _countTime = false;
        _finalTime = _currentTime;
    }
}
