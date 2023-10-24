using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType 
{
    NO_OBJECTIVE = 0,
    ELIMINATION,
    TIME_ELIMINATION,
    RACE
}

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private List<Objective> _objectives = new List<Objective>();
}
