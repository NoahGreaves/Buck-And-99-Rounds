using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private void Start()
    {
        InitEliminationObjective();
    }

    private void InitEliminationObjective()
    {
        GameEvents.GetObjectives(ObjectiveType.ELIMINATION);
    }
}
