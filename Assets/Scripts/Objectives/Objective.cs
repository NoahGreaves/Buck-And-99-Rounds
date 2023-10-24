using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Give enemy Objective GameObjects this component, and add objectives to a list in ObjectiveManager
/// Same can go for any objective with multiple steps i.e: Elimination, Waypoints
/// </summary>
public class Objective : MonoBehaviour
{
    [SerializeField] private string _name = "";
    [SerializeField] protected ObjectiveType objectiveType = ObjectiveType.NO_OBJECTIVE;

    public ObjectiveData ObjectiveData = new ObjectiveData();
    private bool _isComplete = false;



    protected void IsCompleted()
    {
        _isComplete = true;
        GameEvents.ObjectiveComplete(this);
    }
}
