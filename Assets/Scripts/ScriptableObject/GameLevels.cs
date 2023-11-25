using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameLevelScriptableObject", order = 1)]
public class GameLevels : ScriptableObject
{
   [SerializeField] public Scene[] Levels;

}
