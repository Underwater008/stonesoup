using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarApple : Tile
{
    public override void pickUp(Tile tilePickingUsUp)
    {
        base.pickUp(tilePickingUsUp);
        tilePickingUsUp.restoreAllHealth();
        die();
    }
}
