using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Shear : ABX_Tile
{
    public float range;
    List<Tile> _target = new List<Tile>();

    public Animator shearAnimator;

    public override void useAsItem(Tile tileUsingUs)
    {
        shearAnimator.SetTrigger("swing");
        if (_target == null)
            return;
        foreach (Tile tile in _target)
            tile.useAsItem(this);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (!tile.hasTag(TileTags.Plant) && !tile.hasTag(TileTags.Water))
            return;
        _target.Add(tile);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
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
