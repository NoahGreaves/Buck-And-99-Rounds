using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeTrials
{
    // Store in array for OverallTime, Lap1, Lap2, Lap3
    private static readonly float[] _currentPlayerTrackTime = new float[4];

    private const int TRACK_TIME_INDEX = 0;

    public static void SetCurrentPlayerLapTime(int lapNumber, float lapTime)
    {
        if (lapNumber - 1 < 0) 
            return;

        float lastLapTime = _currentPlayerTrackTime[lapNumber - 1];
        switch (lapNumber - 1)
        {
            case 0:
                _currentPlayerTrackTime[lapNumber] = lapTime;
                break;

            case 1:
                float newLapTime = lapTime - lastLapTime;
                _currentPlayerTrackTime[lapNumber] = newLapTime;
                break;

            case 2:
                float firstLapTime = _currentPlayerTrackTime[lapNumber - 2];
                newLapTime = lapTime - (lastLapTime + firstLapTime);
                _currentPlayerTrackTime[lapNumber] = newLapTime;
                break;
        }
    }

    public static void SetCurrentPlayerTackTime(float trackTime)
    { 
        _currentPlayerTrackTime[TRACK_TIME_INDEX] = trackTime;
    }

    public static void ResetCurrentTrackTimes() 
    {
        for (int i = 0; i < _currentPlayerTrackTime.Length; i++)
        {
            _currentPlayerTrackTime[i] = 0.0f;
        }
    }

    public static float GetTrackTimes(int timeIndex)
    {
        return _currentPlayerTrackTime[timeIndex];
    }

    public static string ParseTime(float timeToParse) 
    {
        float mins = Mathf.FloorToInt(timeToParse / 60);
        float secs = Mathf.FloorToInt(timeToParse % 60);
        float centiSeconds = Mathf.FloorToInt((timeToParse % 1f) * 100);

        string time = $"{mins:00}:{secs:00}.{centiSeconds:00}";
        return time;
    }
}
