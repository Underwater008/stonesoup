using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBomb : Tile
{
    public GameObject bombPrefab;

    public override void useAsItem(Tile tileUsingUs)
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
        takeDamage(this, 9999);
    }

    
}
