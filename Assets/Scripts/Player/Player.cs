using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
    public static int LAYER = 3;

    public static float TotalBoostAmount = 0f;

    public static GameObject Model = null;

    public static Health Health = null;

    public static bool CanSpeedKill = false;

    public static bool IsMoving = false;

    public static PlayerVehicle CurrentPlayerVehicle = null;

    public static bool IsGrounded = false;
    public static Vector3 GroundNormal = new Vector3();

    private static bool _canMove = true;
    public static bool CanMove
    {
        get => _canMove;
        set
        {
            _canMove = value;

            // UPDATE UI
            //GameEvents.PlayerLivesUpdate(_numOfLives);
        }
    }

    private static int _numOfLives = 2; // Count from 0;
    public static int NumOfLives
    {
        get => _numOfLives;
        set
        {
            _numOfLives = value;

            // UPDATE UI
            GameEvents.PlayerLivesUpdate(_numOfLives);
        }
    }

    ///
    /// Store and track player data (Health, Damage, Power Ups)
    /// 
}
