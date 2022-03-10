using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23Projectile : Tile
{
    public float moveSpeed = 1;
    public int damageCause;
    [SerializeField]
    [EnumFlagsAttribute]
    public TileTags effectTags;

    public void Start()
    {
        init();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Tile otherTile = collision.gameObject.GetComponent<Tile>();
        if (otherTile != null && otherTile.hasTag(effectTags))
        {
            otherTile.takeDamage(this, damageCause);
            takeDamage(otherTile, 1);
        }
    }
    public void Update()
    {
        moveViaVelocity(transform.right, moveSpeed, 20);
    }
}
