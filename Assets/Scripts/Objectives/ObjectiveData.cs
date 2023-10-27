using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveData
{
    // WHEN ENEMY DIES, REMOVE FROM THE LIST
    private static readonly List<Objective> _eliminationObjectives = new List<Objective>();

    private static ObjectiveType _objectiveType;
    public static ObjectiveType ObjectiveType 
    {
        get => _objectiveType;
        set 
        {
            _objectiveType = value;
        }
    }

    private bool _isComplete = false;
    public bool IsComplete
    {
        get => _isComplete;
        set => _isComplete = value;
    }

    // Constructor
    public ObjectiveData(ObjectiveType objectiveType)
    {
        _objectiveType = objectiveType;
    }

    // Destructor
    ~ObjectiveData()
    {
        _eliminationObjectives.Clear();
    }

    public static void CountObjectives(Objective objective, ObjectiveData data)
    {
        switch (ObjectiveType)
        {
            case ObjectiveType.ELIMINATION:
                SetEliminationObjectives(objective, data);
                break;
            case ObjectiveType.TIME_ELIMINATION:
                break;
            case ObjectiveType.RACE:
                break;
            default:
                break;
        }
    }

    private static void SetEliminationObjectives(Objective objective, ObjectiveData data)
    {
        // if _isCompleted == true, dont add/remove from the list

        if (!data.IsComplete)
        {
            _eliminationObjectives.Add(objective);
        }
        else
        {
            _eliminationObjectives.Remove(objective);
        }
        GameEvents.EnemyCountUpdate(_eliminationObjectives.Count);

        if (_eliminationObjectives.Count <= 0)
        {
            GameEvents.ObjectiveComplete();
        }
    }
}