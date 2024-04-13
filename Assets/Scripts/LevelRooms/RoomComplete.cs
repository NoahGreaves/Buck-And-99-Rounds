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

    private void Start()
    {
        _trigger = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();

        GameEvents.OnRoomObjectivesComplete += RoomCompleted;

        if (_isVisible)
            return;
       
        _renderer.enabled = false;
        _trigger.enabled = false;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomObjectivesComplete -= RoomCompleted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasPlayerEntered && other.gameObject.layer != Player.LAYER)
            return;

        _hasPlayerEntered = true;
        _currentNumOfLaps += 1;
        if (_currentNumOfLaps != _numOfLaps)
        {
            GameEvents.LapCountUpdate(_currentNumOfLaps);
            return;
        }

        GameEvents.TimeTrialTimerEnd();
        GameEvents.RoomProgression(_roomCollection);
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
