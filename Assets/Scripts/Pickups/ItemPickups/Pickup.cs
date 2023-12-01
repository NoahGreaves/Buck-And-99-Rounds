using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    protected bool CompareToPlayerLayer(int layer)
    {
        if (layer == Player.LAYER)
            return true;
        return false;
    }
}
