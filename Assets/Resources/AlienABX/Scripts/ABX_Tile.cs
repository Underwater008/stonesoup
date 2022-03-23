using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Tile : Tile
{
    public virtual void interact(Tile tileIneracting)
    {

    }

    public static ABX_Tile spawnABX_Tile(GameObject tilePrefab, Transform parentOfTile, int gridX, int gridY)
    {
        // Enforce constraints on where we spawn tiles.
        if (gridX < 0 || gridX >= LevelGenerator.ROOM_WIDTH || gridY < 0 || gridY >= LevelGenerator.ROOM_HEIGHT)
        {
            throw new UnityException(string.Format("Attempted to spawn tile outside room boundaries. Tile: {0}, Grid X: {1}, Grid Y: {1}", tilePrefab, gridX, gridY));
        }

        GameObject tileObj = Instantiate(tilePrefab) as GameObject;
        tileObj.transform.parent = parentOfTile;
        ABX_Tile tile = tileObj.GetComponent<ABX_Tile>();
        Vector2 tilePos = toWorldCoord(gridX, gridY);

        tile.localX = tilePos.x;
        tile.localY = tilePos.y;
        tile.init();
        return tile;
    }



    public virtual void remove()
    {
        _alive = false;

        Destroy(gameObject);
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        if (!tilePickingUsUp.hasTag(TileTags.Player))
        {
            base.pickUp(tilePickingUsUp);
            return;
        }
        ABX_xiao23InventorySelect backpack = tilePickingUsUp.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack == null)
        {
            base.pickUp(tilePickingUsUp);
            return;
        }
        /*if (!hasTag(TileTags.Wearable))
        {
            isAdded = backpack.AddItem(this);
        }*/
        int isAdded = backpack.AddItem(this);
        if (isAdded == 0)
        {
            Destroy(gameObject);
        }
        else if (isAdded == 1)
        {
            base.pickUp(tilePickingUsUp);
            gameObject.SetActive(false);
        }
    }

    public override void dropped(Tile tileDroppingUs)
    {
        if (!tileDroppingUs.hasTag(TileTags.Player))
        {
            base.dropped(tileDroppingUs);
            return;
        }
        ABX_xiao23InventorySelect backpack = _tileHoldingUs.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack == null)
        {
            base.dropped(tileDroppingUs);
            return;
        }
        int remain = backpack.ConsumeCurrentItem(1);
        if (remain == 0)
        {
            base.dropped(tileDroppingUs);
        }
        else
        {
            GameObject obj = Instantiate(gameObject, transform.position, Quaternion.identity);
            obj.transform.parent = null;
            obj.GetComponent<ABX_Tile>().addTag(TileTags.CanBeHeld);

        }
    }

    protected override void die()
    {
        if (_tileHoldingUs == null)
        {
            base.die();
            return;
        }
        if (!_tileHoldingUs.hasTag(TileTags.Player))
        {
            base.die();
            return;
        }
        ABX_xiao23InventorySelect backpack = _tileHoldingUs.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack == null || hasTag(TileTags.Player) || !_tileHoldingUs.hasTag(TileTags.Player))
        {
            base.die();
            return;
        }
        int remain = backpack.ConsumeCurrentItem(1);
        if (remain > 0)
        {
            ABX_Tile newTile = Instantiate(gameObject).GetComponent<ABX_Tile>();
            newTile._tileHoldingUs = _tileHoldingUs;
            newTile.die();
        }
        else
        {
            base.die();
        }
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        if (!tileUsingUs.hasTag(TileTags.Player))
        {
            base.useAsItem(tileUsingUs);
            return;
        }
        ABX_xiao23InventorySelect backpack = _tileHoldingUs.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack == null)
        {
            base.useAsItem(tileUsingUs);
            return;
        }
        if (backpack.GetCurrentCount() == 0)
        {
            die();
        }
    }
}
