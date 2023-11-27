using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isTestBuild = true;

    private const int HUB_WORLD_INDEX = 1;
    private const int TEST_ROOM_INDEX = 2;
    private int _currentRoomIndex;
    private int _roomCount;

    private void OnEnable()
    {
        GameEvents.OnRoomProgression += LoadNextRoom;
        _roomCount = SceneManager.sceneCountInBuildSettings - 1;
    }

    private void Start()
    {
        //_currentRoomIndex = STARTING_ROOM_INDEX;
        //if (_isTestBuild)
        //    _currentRoomIndex = TEST_ROOM_INDEX;

        _currentRoomIndex = HUB_WORLD_INDEX;

        LoadInitialRoom();
    }

    private void OnDisable()
    {
        GameEvents.OnRoomProgression -= LoadNextRoom;
    }

    private void LoadInitialRoom()
    {
        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
    }

    private void LoadStartingRoom()
    {
        var roomName = GetCurrentRoomName();
        SceneManager.UnloadSceneAsync(roomName);

        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
    }

    private void LoadNextRoom(RoomCollection roomCollection)
    {
        // _currentLevelIndex = (_currentLevelIndex + 1) % _gameLevels.Levels.Length;
        var roomName = GetCurrentRoomName();
        SceneManager.UnloadSceneAsync(roomName);

        _currentRoomIndex = (int)roomCollection;
        if (_currentRoomIndex > _roomCount || _currentRoomIndex <= 0)
            _currentRoomIndex = HUB_WORLD_INDEX;

        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
    }

    public void RestartRoom()
    {
        var activeScenes = SceneManager.sceneCount - 1;
        var currentLevelScene = SceneManager.GetSceneAt(activeScenes);
        if (_isTestBuild)
        {
            SceneManager.UnloadSceneAsync(currentLevelScene);
            SceneManager.LoadScene(TEST_ROOM_INDEX, LoadSceneMode.Additive);
            return;
        }
        
        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        GameEvents.RoomReset();
    }

    private string GetCurrentRoomName() 
    {
        return SceneManager.GetSceneAt(1).name;
    }
}
