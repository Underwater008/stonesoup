using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Shear : ABX_Tile
{
    public float range;
    List<ABX_Tile> _target = new List<ABX_Tile>();

    public override void useAsItem(Tile tileUsingUs)
    {
        if (_target == null)
            return;
        foreach (ABX_Tile tile in _target)
            tile.useAsItem(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        ABX_Tile tile = collision.gameObject.GetComponent<ABX_Tile>();
        if (tile == null)
            return;
        if (!tile.hasTag(TileTags.Plant) && !tile.hasTag(TileTags.Water))
            return;
        _target.Add(tile);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ABX_Tile tile = collision.gameObject.GetComponent<ABX_Tile>();
        if (tile == null)
            return;
        if (_target.Contains(tile))
            _target.Remove(tile);
    }

    /*void Update()
    {
        if (_target != null)
        {
            if ((_target.gameObject.transform.position - transform.position).magnitude > range)
            {
                _target = null;
            }
        }
    }
    */
}
