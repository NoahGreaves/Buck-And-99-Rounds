using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Give enemy Objective GameObjects this component, and add objectives to a list in ObjectiveManager
/// Same can go for any objective with multiple steps i.e: Elimination, Waypoints
/// </summary>
public class Objective : MonoBehaviour
{
    [SerializeField] private ObjectiveType _objectiveType = ObjectiveType.NO_OBJECTIVE;

    private ObjectiveData _objectiveData;
    private bool _isComplete = false;

    private void Awake()
    {
        _objectiveData = new(_objectiveType);

        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnGetObjectives += GetObjective;
    }

    private void UnsubscribeToEvents()
    {
        GameEvents.OnGetObjectives -= GetObjective;
    }

    private void GetObjective(ObjectiveType objectiveType)
    {
        if (_objectiveType == objectiveType)
        {
            ObjectiveData.CountObjectives(this, _objectiveData);
            return;   
        }
    }

    public void SetIsCompleted(bool isCompleted)
    {
        _objectiveData.IsComplete = isCompleted;

        ObjectiveData.CountObjectives(this, _objectiveData);
    }
}