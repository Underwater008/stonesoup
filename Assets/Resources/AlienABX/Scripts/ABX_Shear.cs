using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Shear : Tile
{
    public float range;
    Tile _target;

    public override void useAsItem(Tile tileUsingUs)
    {
        if (_target == null)
            return;
        _target.useAsItem(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (!tile.hasTag(TileTags.Plant))
            return;
        _target = tile;
    }

    void Update()
    {
        if (_target != null)
        {
            if ((_target.gameObject.transform.position - transform.position).magnitude > range)
            {
                _target = null;
            }
        }
    }
}
