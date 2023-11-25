using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationObjective : MonoBehaviour
{
    private RoomObjective _roomObjective;
    private Objective[] _enemiesInLevel;

    private const ObjectiveType OBJECTIVE_TYPE = ObjectiveType.ELIMINATION;

    private void Start()
    {
        GetEnemiesInLevel();
        _roomObjective = GetComponent<RoomObjective>();

        GameEvents.OnAlertMainObjective += CheckIfComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnAlertMainObjective -= CheckIfComplete;
    }

    private void GetEnemiesInLevel()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        _enemiesInLevel = new Objective[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            _enemiesInLevel[i] = enemies[i].GetComponent<Objective>();
        }
    }

    private void CheckIfComplete(ObjectiveType objectiveType)
    {
        if (objectiveType != OBJECTIVE_TYPE)
            return;

        bool completed = false;
        for (int i = 0; i < _enemiesInLevel.Length; i++)
        {
            var enemy = _enemiesInLevel[i];
            completed = enemy.IsComplete;
            if (completed)
                continue;
            else
                break;
        }

        print($"Objective is Complete: {completed}");
        _roomObjective.IsComplete = completed;
        if (completed)
        {
            GameEvents.AlertRoomObjectiveComplete();
        }
    }
}
