using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Shovel : ABX_Tile
{
    const float UPTIME = 0.2f;
    CircleCollider2D _pickupCol;
    BoxCollider2D _groundCol;

    private void Awake()
    {
        _pickupCol = GetComponent<CircleCollider2D>();
        _groundCol = GetComponent<BoxCollider2D>();
    }
    public override void useAsItem(Tile tileUsingUs)
    {
        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(_tileHoldingUs.transform.position, Vector3.down);
        foreach (RaycastHit2D hit2D in hit)
        {
            Tile tile = hit2D.transform.GetComponent<Tile>();
            if (tile == null)
                continue;
            if (tile.hasTag(TileTags.Dirt))
            {
                tile.useAsItem(this);
            }
        }
    }

    IEnumerator UseShovel ()
    {
        _pickupCol.enabled = true;
        yield return new WaitForSeconds(UPTIME);
        _pickupCol.enabled = false;
    }

    void FixedUpdate()
    {
        if (_tileHoldingUs != null)
        {
            // Let's try to rotate towards the aim direction. 
            float aimAngle = Mathf.Atan2(_tileHoldingUs.aimDirection.y, _tileHoldingUs.aimDirection.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, 0, aimAngle);
        }
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        _groundCol.enabled = false;
        base.pickUp(tilePickingUsUp);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ABX_Tile tile = collision.gameObject.GetComponent<ABX_Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Dirt))
        {
            tile.useAsItem(this);
        }
    }
}
