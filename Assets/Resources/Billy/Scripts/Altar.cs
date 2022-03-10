using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : Tile
{
    public int damageToCreature;
    public int damageToSelf;
    public GameObject[] dropsPrefab;
    public GameObject deathDropPrefab;

    float iFrames = 0;
    public float totalIFrames;

    void Update()
    {
        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (iFrames > 0)
        {
            return;
        }
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.hasTag(TileTags.Creature))
            {
                if (tile.tileWereHolding != null)
                {
                    tile.takeDamage(this, damageToCreature);
                    tile.tileWereHolding.takeDamage(this, 9999);
                    GameObject newItem = Instantiate(GlobalFuncs.randElem(dropsPrefab), gameObject.transform.parent);
                    newItem.GetComponent<Tile>().pickUp(tile);
                    takeDamage(this, damageToSelf);
                    iFrames = totalIFrames;
                }
            }
        }
    }

    protected override void die()
    {
        GameObject item = Instantiate(deathDropPrefab, gameObject.transform.parent);
        item.transform.position = transform.position;

        base.die();
    }

}
