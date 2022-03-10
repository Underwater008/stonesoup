using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Shovel : Tile
{
    public override void useAsItem(Tile tileUsingUs)
    {
        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(_tileHoldingUs.transform.position, Vector3.down);
        if (hit.Length == 0)
        {
        }
        foreach (RaycastHit2D hit2D in hit)
        {
            Tile tile = hit2D.transform.GetComponent<Tile>();
            if (tile == null)
                continue;
            if (tile.hasTag(TileTags.Dirt))
            {
                tile.useAsItem(this);
            }
        }
    }
}
