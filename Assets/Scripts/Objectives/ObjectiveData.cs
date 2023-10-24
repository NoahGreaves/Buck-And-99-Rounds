using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveData
{
    private static int _numOfObjectives = 0;
    public static int numOfObjectives
    {
        private set => _numOfObjectives = value;
        get => _numOfObjectives;
    }

    public string name;
    private static ObjectiveData _objectiveData = new ObjectiveData();

    // Constructor
    public ObjectiveData()
    {

    }

    // Destructor
    ~ObjectiveData()
    {

    }

    // toAdd == true at the start of the game to ADD all the objectives together. 
    // as objectives are acomplete toAdd should be set to false, and the completed
    // objectes will be removed from the count i.e: Elimination, Waypoints for Racing
    public static void CountObjectives(bool toAdd)
    {
        numOfObjectives = toAdd ? numOfObjectives + 1 : numOfObjectives - 1;
        Debug.Log(numOfObjectives + " " + toAdd);
    }
}