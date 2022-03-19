using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23EnemyShrem : BasicAICreature
{

    Tile _player;
    public void Update()
    {
        if(_player !=null)
        {
            var dir = _player.transform.position - transform.position;
            dir.Normalize();

            moveViaVelocity(dir, 2, 5);
        }
     
    }

    public override void FixedUpdate()
    {
        
    }
    public override void tileDetected(Tile detectedTile)
    {
        base.tileDetected(detectedTile);
        _player = detectedTile;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Tile otherTile = collision.transform.GetComponent<Tile>();
        if (otherTile != null && otherTile.hasTag(TileTags.Player))
        {
            otherTile.takeDamage(this, 1);
        }
    }
}
