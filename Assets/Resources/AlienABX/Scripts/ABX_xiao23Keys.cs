using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_xiao23Keys : ABX_Tile
{


    public override void pickUp(Tile tilePickingUsUp)
    {

        if (tileName == "BronzeKey")
        {
            Debug.Log("Picked up BronzeKey");

        }

        if (tileName == "SilverKey")
        {
            Debug.Log("Picked up SilverKey");

        }

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


        //base.pickUp(tilePickingUsUp);

        //tilePickingUsUp.restoreAllHealth();

    }
    public override void useAsItem(Tile tileUsingUs)
    {
        base.useAsItem(tileUsingUs);
        if (!(tileUsingUs is xiao23Doors))
            return;
        Destroy(gameObject);
        if (xiao23InventorySelect.MainInventory != null)
        {
            xiao23InventorySelect.MainInventory.RemoveItem(this);
        }

    }

    public override void dropped(Tile tileDroppingUs)
    {
        base.dropped(tileDroppingUs);
        if (xiao23InventorySelect.MainInventory != null)
        {
            xiao23InventorySelect.MainInventory.RemoveItem(this);
        }
    }
    public void OnDisable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
