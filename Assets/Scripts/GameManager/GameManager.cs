using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isTestBuild = true;

    private const int TEST_LEVEL_INDEX = 1;
    private int _currentLevelIndex = 2;

    private void OnEnable()
    {
        GameEvents.OnRoomProgression += LoadNextLevel;
    }

    private void Start()
    {
        if (_isTestBuild)
            _currentLevelIndex = TEST_LEVEL_INDEX;

        LoadInitialLevel();
    }

    private void OnDisable()
    {
        GameEvents.OnRoomProgression -= LoadNextLevel;
    }

    private void LoadInitialLevel()
    {
        SceneManager.LoadScene(_currentLevelIndex, LoadSceneMode.Additive);
    }

    private void LoadNextLevel()
    {
        // _currentLevelIndex = (_currentLevelIndex + 1) % _gameLevels.Levels.Length;
        var sceneName = SceneManager.GetSceneAt(1).name;
        SceneManager.UnloadSceneAsync(sceneName);

        _currentLevelIndex += 1;
        SceneManager.LoadScene(_currentLevelIndex, LoadSceneMode.Additive);
    }

    public void RestartLevel()
    {
        var activeScenes = SceneManager.sceneCount - 1;
        var currentLevelScene = SceneManager.GetSceneAt(activeScenes);
        if (_isTestBuild)
        {
            SceneManager.UnloadSceneAsync(currentLevelScene);
            SceneManager.LoadScene(TEST_LEVEL_INDEX, LoadSceneMode.Additive);
            return;
        }
        SceneManager.LoadScene(_currentLevelIndex, LoadSceneMode.Additive);
    }
}
