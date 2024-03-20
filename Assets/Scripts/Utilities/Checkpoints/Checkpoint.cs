using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 Position => transform.position;

    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager.SetPlayerCheckpoint(this);
    }
}
