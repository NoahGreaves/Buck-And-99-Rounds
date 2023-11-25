using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    // add objetive in levels to dictionary. progress level when all objectives are marked as complete
    private Dictionary<ObjectiveType, RoomObjective> _roomObjectives = new Dictionary<ObjectiveType, RoomObjective>();
    private bool _roomObjectivesComplete = false;

    private void Start()
    {
        GameEvents.OnAlertRoomObjectiveComplete += CheckRoomObjectiveCompletion;

        InitEliminationObjective();
        GetObjectives();
    }

    private void OnDisable()
    {
        GameEvents.OnAlertRoomObjectiveComplete -= CheckRoomObjectiveCompletion;
    }

    private void InitEliminationObjective()
    {
        GameEvents.GetObjectives(ObjectiveType.ELIMINATION);
    }

    private void GetObjectives() 
    {
        var roomObjs = gameObject.GetComponentsInChildren<RoomObjective>();
        for (int i = 0; i < roomObjs.Length; i++) 
        {
            var obj = roomObjs[i].RoomObjectiveType;
            _roomObjectives.Add(obj, roomObjs[i]);
        }
    }

    private void CheckRoomObjectiveCompletion() 
    {
        bool isRoomComplete = false;
        foreach (var roomObj in _roomObjectives.Values)
        {
            isRoomComplete = roomObj.IsComplete;
            if (isRoomComplete)
                continue;
            else
                break;
        }

        _roomObjectivesComplete = isRoomComplete;
        
        if (_roomObjectivesComplete) 
        {
            GameEvents.RoomObjectivesComplete();
        }
    }
}
