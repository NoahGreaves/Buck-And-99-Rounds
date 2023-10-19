using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player 
{
    public static float MovementSpeed = 10;
    private static float _defaultMovementSpeed = 100; // read from json ?
    
    public static bool CanSpeedKill = false;

    public static Vehicle CurrentPlayerVehicle = null;

    public static float SetMoveSpeed(float speed) => MovementSpeed = speed;
    public static float ResetMovementSpeed() => MovementSpeed = _defaultMovementSpeed;

    ///
    /// Store and track player data (Health, Damage, Power Ups), and make
    /// a seperate components that are responsible for animations, enemy interactions,
    /// and environemnt interactions
    ///
}
