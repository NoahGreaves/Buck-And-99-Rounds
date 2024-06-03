using UnityEngine;

public enum RoomCollection
{
    NO_ROOM = 0,
    HUB_ROOM,
    TEST_ROOM,
    ROOM_LEVEL_1,
    ROOM_LEVEL_2,
    CITY
}

public class RoomComplete : MonoBehaviour
{
    [SerializeField] private RoomCollection _roomCollection;
    [SerializeField] private bool _isVisible = false;

    [SerializeField] private int _numOfLaps = 3;

    private Collider _trigger;
    private MeshRenderer _renderer;

    private bool _hasPlayerEntered = false;
    private int _currentNumOfLaps = 0;

    private const int HUB_ROOM_INDEX = (int)RoomCollection.HUB_ROOM;

    private void Start()
    {
        _trigger = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();

        GameEvents.OnRoomObjectivesComplete += RoomCompleted;

        if (_isVisible)
            return;
       
        _renderer.enabled = false;
        _trigger.enabled = false;

        _currentNumOfLaps = 0;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomObjectivesComplete -= RoomCompleted;
        _currentNumOfLaps = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasPlayerEntered && other.gameObject.layer != Player.LAYER)
            return;

        _hasPlayerEntered = true;

        _currentNumOfLaps += 1;
        GameEvents.GetCurrentLapTime(_currentNumOfLaps);

        if (_currentNumOfLaps != _numOfLaps)
        {
            GameEvents.LapCountUpdate(_currentNumOfLaps);
            return;
        }

        // Stop the Time Trials timer
        GameEvents.TimeTrialTimerEnd();

        // Save the Time for the Total Time to Complete Track
        GameEvents.GetCurrentTrackTime();

        // Display the Leaderboard
        int currentLevelIndex = GameManager.GetCurrentRoomIndex();
        if (currentLevelIndex == HUB_ROOM_INDEX)
        {
            // Move to next level
            GameEvents.RoomProgression(_roomCollection);
            return;
        }

        GameEvents.SetLeaderboardStatus(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_hasPlayerEntered && other.gameObject.layer != Player.LAYER)
            return;

        _hasPlayerEntered = false;
    }

    private void RoomCompleted()
    {
        _trigger.enabled = true;
        _renderer.enabled = true;
    }
}
