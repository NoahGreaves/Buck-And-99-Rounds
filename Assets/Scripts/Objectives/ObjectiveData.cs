using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveData
{
    // WHEN ENEMY DIES, REMOVE FROM THE LIST
    private static readonly List<Objective> _eliminationObjectives = new List<Objective>();

    private static int _numOfObjectives = 0;
    public static int numOfObjectives
    {
        private set => _numOfObjectives = value;
        get => _numOfObjectives;
    }

    private static ObjectiveType _objectiveType;
    public static ObjectiveType ObjectiveType 
    {
        get => _objectiveType;
        set 
        {
            _objectiveType = value;
        }
    }

    private bool _isCompleted = false;
    public bool IsComplete
    {
        get => _isCompleted;
        set
        {

        }
    }

    // Constructor
    public ObjectiveData(ObjectiveType objectiveType)
    {
        _objectiveType = objectiveType;
    }

    // Destructor
    ~ObjectiveData()
    {

    }

    // toAdd == true at the start of the game to ADD all the objectives together. 
    // as objectives are acomplete toAdd should be set to false, and the completed
    // objectes will be removed from the count i.e: Elimination, Waypoints for Racing
    public static void CountObjectives(Objective objective)
    {
        switch (ObjectiveType) 
        {
            case ObjectiveType.ELIMINATION:
                SetEliminationObjectives(objective);
                break;
            case ObjectiveType.TIME_ELIMINATION:
                break;
            case ObjectiveType.RACE:
                break;
            default:
                break;
        }
    }

    private static void SetEliminationObjectives(Objective objective)
    {
        // FIND A WAY TO KNOW IF ENEMY IS ALIVE OR DEAD]
        // IF ALIVE -> ADD TO LIST
        // IF DEAD  -> REMOVE FROM LIST

        //if () { }
        _eliminationObjectives.Add(objective);
        GameEvents.EnemyCountUpdate(_eliminationObjectives.Count);
    }
}