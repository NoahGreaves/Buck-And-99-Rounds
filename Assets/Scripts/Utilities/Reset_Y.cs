using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Y : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager.MovePlayerToCurrentCheckpoint();
    }
}
