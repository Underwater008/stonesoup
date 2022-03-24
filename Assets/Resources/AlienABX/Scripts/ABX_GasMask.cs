using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_GasMask : ABX_Tile
{
    public override void pickUp(Tile tilePickingUsUp)
    {
        if (!tilePickingUsUp.hasTag(TileTags.Player))
            return;
        transform.parent = tilePickingUsUp.transform;
        transform.localPosition = new Vector3(0, 0.3f, 0);
        ABX_Player player = (ABX_Player)tilePickingUsUp;
        player.hasGasMask = true;
    }
}
