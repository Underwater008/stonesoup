using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Dirt : Tile
{
    public Sprite barrenSprite;
    public Sprite enrichedSprite;
    public float enrichInterval;
    public float enrichRate;

    public override void init()
    {
        StartCoroutine(Dirt());
        base.init();
    }

    IEnumerator Dirt ()
    {
        yield return new WaitForSeconds(enrichInterval);
        if (Random.Range(0f, 1f) < enrichRate)
        {
            Enrich();
        }
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        sprite.sprite = barrenSprite;
        removeTag(TileTags.Dirt);
        //Drop items
    }

    void Enrich ()
    {
        sprite.sprite = enrichedSprite;
        addTag(TileTags.Dirt);
    }
}
