using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Dirt : ABX_Tile
{
    [Header("Dirt")]
    public Sprite barrenSprite;
    public Sprite enrichedSprite;
    public float enrichInterval;
    public float enrichRate;

    [Header("Drops")]
    public GameObject commonDrop;
    public GameObject rareDrop;
    public float commonChance;
    public float rareChance;
    public int minDrop;
    public int maxDrop;
    public float offsetRange;


    public override void init()
    {
        StartCoroutine(Dirt());
        base.init();
    }

    IEnumerator Dirt()
    {
        yield return new WaitForSeconds(enrichInterval);
        if (Random.Range(0f, 1f) < enrichRate)
        {
            Enrich();
        }
        StartCoroutine(Dirt());
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        sprite.sprite = barrenSprite;
        removeTag(TileTags.Dirt);
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
        StopAllCoroutines();
        StartCoroutine(Dirt());
        if (deathSFX != null)
            AudioManager.playAudio(deathSFX);
    }

    void Enrich()
    {
        sprite.sprite = enrichedSprite;
        addTag(TileTags.Dirt);
    }
}
