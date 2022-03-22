using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_IndestructableWall : ABX_Tile
{
    public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
    {
        // We're indestructible, ignore all damage.
    }
}
