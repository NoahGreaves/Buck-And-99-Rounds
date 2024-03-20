using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    private static Checkpoint _currentCheckpoint;

    public static void SetPlayerCheckpoint(Checkpoint checkpoint) 
    {
        _currentCheckpoint = checkpoint;
    }

    public static void MovePlayerToCurrentCheckpoint()
    {
        Player.CurrentPlayerVehicle.transform.position = _currentCheckpoint.Position;
    }
}
