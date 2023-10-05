using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player 
{
    public static float MovementSpeed = 100;
    private static float _defaultMovementSpeed = 100; // read from json ?

    public static float SetMoveSpeed(float speed) => MovementSpeed = speed;
    public static float ResetMovementSpeed() => MovementSpeed = _defaultMovementSpeed;

    public static PlayerInputActions Controls = new PlayerInputActions();

    ///
    /// Store and track player data (Health, Damage, Power Ups), and make
    /// a seperate components that are responsible for animations, enemy interactions,
    /// and environemnt interactions
    ///
}
