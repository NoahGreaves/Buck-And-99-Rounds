using UnityEngine;

public class TimeTrialsTimer : MonoBehaviour
{
    [SerializeField] private UIController _uiController;

    private bool _countTime = false;
    private float _currentTime = 0f;
    
    private void Start()
    {
        GameEvents.OnTimeTrialTimerStart += StartTimer;
        GameEvents.OnTimeTrialTimerEnd += EndTimer;

        GameEvents.OnGetCurrentLapTime += SetPlayerLapTime;
        GameEvents.OnGetCurrentTrackTime += SetPlayerTrackTime;

        // Set initial time 00:00:00
        _uiController.UpdateTimeTrialTimer(_currentTime);
    }

    private void OnDisable()
    {
        GameEvents.OnTimeTrialTimerStart -= StartTimer;   
        GameEvents.OnTimeTrialTimerEnd -= EndTimer;

        GameEvents.OnGetCurrentLapTime -= SetPlayerLapTime;
        GameEvents.OnGetCurrentTrackTime -= SetPlayerTrackTime;
    }

    private void Update()
    {
        if (!_countTime)
            return;

        _currentTime += Time.deltaTime;
        _uiController.UpdateTimeTrialTimer(_currentTime);
    }

    private void StartTimer() 
    {
        TimeTrials.ResetCurrentTrackTimes();
        ResetTimer();
        _countTime = true;
    }

    // Get the amount of time the player too to complete one lap of the current track
    private void SetPlayerLapTime(int currentLap)
    {
        var lapTime = _currentTime;
        TimeTrials.SetCurrentPlayerLapTime(currentLap, lapTime);
    }

    // Get the amount of time the player took to complete Total Number of Laps per Track
    private void SetPlayerTrackTime() 
    {
        var lapTime = _currentTime;
        TimeTrials.SetCurrentPlayerTackTime(lapTime);
    }

    private void EndTimer() 
    {
        _countTime = false;
    }

    private void ResetTimer() 
    {
        _currentTime = 0.0f;
        _uiController.UpdateTimeTrialTimer(_currentTime);
    }
}
