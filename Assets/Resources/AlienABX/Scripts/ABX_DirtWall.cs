using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_DirtWall : ABX_Tile
{
    [Header("Drop")]
    public GameObject dropItemPrefab;
    public float dropChance;

    public override void useAsItem(Tile tileUsingUs)
    {
        if (Random.Range(0f, 1f) < dropChance)
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        takeDamage(tileUsingUs, 9999);
    }
}
