using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Tile
{
    public Collider2D col;
    bool _isDigging = false;
    bool _canDig = false;

    public override void useAsItem(Tile tileUsingUs)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Dirt))
        {
            tile.useAsItem(this);
        }
    }
}
