using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _currentRoomIndex;
    private int _initRoomIndex;
    private int _roomCount;

    private void OnEnable()
    {
        GameEvents.OnRoomProgression += LoadNextRoom;
        _roomCount = SceneManager.sceneCountInBuildSettings - 1;
    }

    private void Start()
    {
        _currentRoomIndex = (int)RoomCollection.HUB_ROOM;
        _initRoomIndex = _currentRoomIndex;

        LoadInitialRoom();
    }

    private void OnDisable()
    {
        GameEvents.OnRoomProgression -= LoadNextRoom;
    }

    #region SceneLoading
    private void LoadInitialRoom()
    {
        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        GameEvents.RoomLoad();
    }

    private void LoadNextRoom(RoomCollection roomCollection)
    {
        // _currentLevelIndex = (_currentLevelIndex + 1) % _roomCount;
        var room = GetCurrentRoomIndex();
        SceneManager.UnloadSceneAsync(room);

        _currentRoomIndex = (int)roomCollection;
        if (_currentRoomIndex > _roomCount || _currentRoomIndex <= 0)
            _currentRoomIndex = (int)RoomCollection.HUB_ROOM;

        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);
        GameEvents.RoomLoad();

    }

    public void RestartRoom()
    {
        var currentRoom = GetCurrentRoomIndex();
        SceneManager.UnloadSceneAsync(currentRoom);

        if (Player.NumOfLives < 0)
        {
            SceneManager.LoadScene(_initRoomIndex, LoadSceneMode.Additive);
            GameEvents.RoomLoad();
            return;
        }

        SceneManager.LoadScene(_currentRoomIndex, LoadSceneMode.Additive);

        GameEvents.RoomLoad();
        GameEvents.RoomReset();
    }

    private int GetCurrentRoomIndex() => SceneManager.GetSceneAt(1).buildIndex;
    #endregion
}