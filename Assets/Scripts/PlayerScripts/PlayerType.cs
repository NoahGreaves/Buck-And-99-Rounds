using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    NULL = 0,
    PLAYER,
    DECOY
}

public class PlayerTypeObject : MonoBehaviour
{
    public PlayerType PLAYERTYPE = PlayerType.PLAYER;
}
