using UnityEngine;

public class RoomComplete : MonoBehaviour
{
    private Collider _trigger;
    private MeshRenderer _renderer;

    private void Start()
    {
        _trigger = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();

        _renderer.enabled = false;
        _trigger.enabled = false;

        GameEvents.OnRoomObjectivesComplete += RoomCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomObjectivesComplete -= RoomCompleted;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameEvents.RoomProgression();
    }

    private void RoomCompleted() 
    {
        _trigger.enabled = true;
        _renderer.enabled = true;
    }
}
