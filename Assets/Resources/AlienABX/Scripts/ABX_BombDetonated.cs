using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_BombDetonated : ABX_Tile
{
    public GameObject minBombPrefab;
    SpriteRenderer spr;

    public int loopNum;
    public float countdown;
    public int damage;
    public Sprite explosionSpr;
    const float _explosionWait = 0.3f;
    const float _brightness = 0.5f;



    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        StartCoroutine(Detonate());
    }
    IEnumerator Detonate()
    {
        for (int i = 0; i < loopNum; i++)
        {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, _brightness);
            yield return new WaitForSeconds(countdown / (loopNum * 2));
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1f);
            yield return new WaitForSeconds(countdown / (loopNum * 2));
        }
        spr.sprite = explosionSpr;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.1f);
        yield return new WaitForSeconds(_explosionWait);
        takeDamage(this, 9999);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.GetComponent<Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Wall) ||
            tile.hasTag(TileTags.Creature))
        {
            if (tile.health <= damage && tile.hasTag(TileTags.Creature))
            {
                Instantiate(minBombPrefab, tile.gameObject.transform.position, Quaternion.identity);
            }
            tile.takeDamage(this, damage, DamageType.Explosive);

        }
    }
}
