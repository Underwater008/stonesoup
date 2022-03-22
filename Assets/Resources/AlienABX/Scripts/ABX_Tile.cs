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
        bool isAdded = false;
        ABX_xiao23InventorySelect backpack = tilePickingUsUp.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack != null && !hasTag(TileTags.Wearable))
        {
            isAdded = backpack.AddItem(this);
        }
        base.pickUp(tilePickingUsUp);
        if (isAdded && backpack.GetItemCount(this) > 1)
            Destroy(gameObject);
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        ABX_xiao23InventorySelect backpack = _tileHoldingUs.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack != null)
        {
            if (backpack.ConsumeCurrentItem(1) == 0)
            {
                die();
            }
        }
        base.useAsItem(tileUsingUs);
    }

    public override void dropped(Tile tileDroppingUs)
    {
        ABX_xiao23InventorySelect backpack = _tileHoldingUs.gameObject.GetComponentInChildren<ABX_xiao23InventorySelect>();
        if (backpack != null)
        {
            backpack.ConsumeCurrentItem(1);
        }
        base.dropped(tileDroppingUs);
    }
}
