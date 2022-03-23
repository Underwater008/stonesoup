using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_xiao23HealingAll : ABX_Tile
{

    public override void pickUp(Tile tilePickingUsUp)
    {
        /*
        Debug.Log("Picked up Apple");

        if (!hasTag(TileTags.CanBeHeld))
        {
            return;
        }

        if (tilePickingUsUp is ABX_xiao23InventorySelect || xiao23InventorySelect.MainInventory == null)
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

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Potion used");
            tilePickingUsUp.restoreAllHealth();
            die();
        }
        */
        base.pickUp(tilePickingUsUp);


    }
    public override void useAsItem(Tile tileUsingUs)
    {
        /*Destroy(gameObject);
        if (xiao23InventorySelect.MainInventory != null)
        {
            xiao23InventorySelect.MainInventory.RemoveItem(this);
        }
        base.useAsItem(tileUsingUs);*/
        if (_tileHoldingUs.hasTag(TileTags.Player))
        {
            tileUsingUs.restoreAllHealth();
            int remain = ABX_xiao23InventorySelect.MainInventory.ConsumeCurrentItem(1);
            if (remain == 0)
            {
                die();
            }
        }
        else
        {
            tileUsingUs.restoreAllHealth();
            die();
        }
    }

    /*
    public override void dropped(Tile tileDroppingUs)
    {
        base.dropped(tileDroppingUs);
        if (xiao23InventorySelect.MainInventory != null)
        {
            xiao23InventorySelect.MainInventory.RemoveItem(this);
        }
    }
    */
}
