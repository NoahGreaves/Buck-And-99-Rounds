using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _timeTexts = new TextMeshProUGUI[4];

    private void OnEnable()
    {
        SetTimeText();
    }

    private void SetTimeText() 
    {
        for (int i = 0; i < _timeTexts.Length; i++)
        {
            float currentTime = TimeTrials.GetTrackTimes(i);
            string parsedTime = TimeTrials.ParseTime(currentTime);            

            _timeTexts[i].text = parsedTime;
        }
    }
}
