using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Plant : Tile
{
    [Header("Plant")]
    public Sprite harvestedSprite;

    [Header("Drops")]
    public GameObject commonDrop;
    public GameObject rareDrop;
    public float rareChance;
    public int minDrop;
    public int maxDrop;
    public float offsetRange;


    bool _canDrop = true;

    public override void useAsItem(Tile tileUsingUs)
    {
        if (!_canDrop)
            return;

        for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
        {
            Vector3 offsetVector = new Vector3(Random.Range(-offsetRange, offsetRange),
                                               Random.Range(-offsetRange, offsetRange),
                                               0);
            if (Random.Range(0f, 1f) < rareChance)
            {
                Instantiate(rareDrop, transform.position + offsetVector, Quaternion.identity);
            }
            else
            {
                Instantiate(commonDrop, transform.position + offsetVector, Quaternion.identity);
            }
        }

        _canDrop = false;
        GetComponent<SpriteRenderer>().sprite = harvestedSprite;
    }
}
