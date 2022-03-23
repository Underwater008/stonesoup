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
        takeDamage(tileUsingUs, 9999, DamageType.Explosive);
    }

    protected override void die()
    {
        if (dropItemPrefab != null && Random.Range(0f, 1f) < dropChance)
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        base.die();
    }

    public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
    {
        if (damageType != DamageType.Explosive)
            return;
        base.takeDamage(tileDamagingUs, damageAmount, damageType);
    }
}
