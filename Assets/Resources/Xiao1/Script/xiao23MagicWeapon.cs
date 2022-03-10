using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23MagicWeapon : Tile
{
    public GameObject projectilePrefab;
    public override void pickUp(Tile tilePickingUsUp)
    {
        if (!hasTag(TileTags.CanBeHeld))
        {
            return;
        }

        if (tilePickingUsUp is xiao23InventorySelect || xiao23InventorySelect.MainInventory == null)
        {
            transform.parent = Player.instance.transform;
            transform.localPosition = new Vector3(heldOffset.x, heldOffset.y, -0.1f);
            transform.localRotation = Quaternion.Euler(0, 0, heldAngle);
            removeTag(TileTags.CanBeHeld);
            Player.instance.tileWereHolding = this;
            _tileHoldingUs = Player.instance;
            updateSpriteSorting();
        }
        else
        {
            xiao23InventorySelect.MainInventory.PutItemInBag(this);
            Destroy(gameObject);
        }
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        base.useAsItem(tileUsingUs);
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dir.Normalize();
        var projectile = Instantiate(projectilePrefab, transform.position+ (Vector3)dir*2, Quaternion.identity);
     

     
        projectile.transform.right = dir;
    }
}
