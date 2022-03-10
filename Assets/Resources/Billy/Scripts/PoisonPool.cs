using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPool : Tile
{
    //How much damage should the poison pool do
    public int damage = 1;
    //How long should the poison pool last
    public float duration = 5;

    void Start()
    {
        StartCoroutine(DestroySelf(duration));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.hasTag(TileTags.Creature))
            {
                tile.takeDamage(this, damage);
            }
        }
    }

    IEnumerator DestroySelf (float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
