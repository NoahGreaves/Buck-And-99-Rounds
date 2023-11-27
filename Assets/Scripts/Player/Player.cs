using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player 
{
    public static int LAYER = 3;

    public static Health Health = null;

    public static bool CanSpeedKill = false;

    public static bool IsMoving = false;

    public static Vehicle CurrentPlayerVehicle = null;

    ///
    /// Store and track player data (Health, Damage, Power Ups), and make
    /// a seperate components that are responsible for animations, enemy interactions,
    /// and environemnt interactions
    ///
}
