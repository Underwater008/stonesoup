using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_DirtWall : ABX_Tile
{
    [Header("Drops")]
    public GameObject commonDrop;
    public GameObject rareDrop;
    public float commonChance;
    public float rareChance;
    public int minDrop;
    public int maxDrop;
    public float offsetRange = 1;

    public override void useAsItem(Tile tileUsingUs)
    {
        takeDamage(tileUsingUs, 9999, DamageType.Explosive);
    }

    protected override void die()
    {
        for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
        {
            Vector3 offsetVector = new Vector3(Random.Range(-offsetRange, offsetRange),
                                               Random.Range(-offsetRange, offsetRange),
                                               0);
            if (Random.Range(0f, 1f) < rareChance)
            {
                Instantiate(rareDrop, transform.position + offsetVector, Quaternion.identity);
            }
            else if (Random.Range(0f, 1f) < commonChance)
            {
                Instantiate(commonDrop, transform.position + offsetVector, Quaternion.identity);
            }
        }
        base.die();
    }

    public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
    {
        if (damageType != DamageType.Explosive)
            return;
        base.takeDamage(tileDamagingUs, damageAmount, damageType);
    }
}
