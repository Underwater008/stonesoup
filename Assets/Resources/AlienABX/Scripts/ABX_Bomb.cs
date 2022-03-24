using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Bomb : ABX_Tile
{
    public GameObject bombPrefab;

    public override void useAsItem(Tile tileUsingUs)
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
        if (_tileHoldingUs.hasTag(TileTags.Player))
        {
            int remain = ABX_xiao23InventorySelect.MainInventory.ConsumeCurrentItem(1);
            if (remain == 0)
            {
                die();
            }
        }
        else
        {
            die();
        }
    }
}
