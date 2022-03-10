using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Tile
{
    public SpriteRenderer spr;
    public Collider2D col;
    public Sprite pickupSprite;
    public Sprite inHandSprite;
    [Header("Force Field")]
    public float cooldown;
    public float uptime;
    public int damage;

    bool _canUse = true;

    
    public override void useAsItem(Tile tileUsingUs)
    {
        StartCoroutine(UseForceField());
    }

    IEnumerator UseForceField ()
    {
        if (!_canUse)
            yield break;

        spr.sprite = inHandSprite;
        col.enabled = true;
        _canUse = false;
        yield return new WaitForSeconds(uptime);

        spr.sprite = pickupSprite;
        col.enabled = false;
        yield return new WaitForSeconds(cooldown);

        _canUse = true;
    }

    public override void dropped(Tile tileDroppingUs)
    {
        spr.sprite = pickupSprite;
        col.enabled = true;
        _canUse = false;
        base.dropped(tileDroppingUs);
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        spr.sprite = pickupSprite;
        col.enabled = false;
        _canUse = true;
        base.pickUp(tilePickingUsUp);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Creature))
        {
            if (!tile.hasTag(TileTags.Friendly))
            {
                tile.takeDamage(this, damage);
            }
        }
        else if (!tile.hasTag(TileTags.Wall))
        {
            tile.takeDamage(this, damage);
        }
    }
}
