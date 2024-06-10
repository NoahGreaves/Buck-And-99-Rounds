using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Y : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != Player.LAYER)
            return;            

        CheckpointManager.MovePlayerToCurrentCheckpoint();
    }
}
