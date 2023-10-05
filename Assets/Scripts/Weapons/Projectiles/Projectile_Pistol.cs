using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Pistol : Projectile
{
    private void FixedUpdate()
    {
        RB.velocity = ( transform.forward * Speed ) * Time.deltaTime;
    }
}
