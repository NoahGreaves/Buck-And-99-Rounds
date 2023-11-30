using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isTestBuild = true;

    private int _currentRoomIndex;
    private int _roomCount;

    private static Vehicle _playerVehicle;
    public static Vehicle PlayerVehicle
    {
        get => _playerVehicle;
        set 
        {
            _playerVehicle = value;
            
            // Whatever else will need to happen when player vehcile is set
        }
    }

    private void OnEnable()
    {
        GameEvents.OnRoomProgression += LoadNextRoom;
        _roomCount = SceneManager.sceneCountInBuildSettings - 1;
    }

    private void Start()
    {
        _currentRoomIndex = (int)RoomCollection.HUB_ROOM;

        LoadInitialRoom();
        ResetPlayerPosition();
    }

    private void OnDisable()
    {
        GameEvents.OnRoomProgression -= LoadNextRoom;
    }

    private void LoadInitialRoom()
    {
        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        ResetPlayerPosition();
    }

    private void LoadNextRoom(RoomCollection roomCollection)
    {
        // _currentLevelIndex = (_currentLevelIndex + 1) % _gameLevels.Levels.Length;
        var roomName = GetCurrentRoomName();
        SceneManager.UnloadSceneAsync(roomName);

        _currentRoomIndex = (int)roomCollection;
        if (_currentRoomIndex > _roomCount || _currentRoomIndex <= 0)
            _currentRoomIndex = (int)RoomCollection.HUB_ROOM;

        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        ResetPlayerPosition();
    }

    public void RestartRoom()
    {
        var activeScenes = SceneManager.sceneCount - 1;
        var currentLevelScene = SceneManager.GetSceneAt(activeScenes);
        if (_isTestBuild)
        {
            SceneManager.UnloadSceneAsync(currentLevelScene);
            SceneManager.LoadScene((int)RoomCollection.TEST_ROOM, LoadSceneMode.Additive);
            return;
        }
        
        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        ResetPlayerPosition();
        GameEvents.RoomReset();
    }

    private void ResetPlayerPosition() 
    {
        PlayerVehicle.transform.position = new Vector3(0, Player.CurrentPlayerVehicle.gameObject.transform.position.y, 0);
    }

    private string GetCurrentRoomName() 
    {
        return SceneManager.GetSceneAt(1).name;
    }
}
