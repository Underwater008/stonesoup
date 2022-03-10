using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbyTile : Tile
{
    public virtual void interact(Tile tileIneracting)
    {

    }

    public static AbbyTile spawnAbbyTile(GameObject tilePrefab, Transform parentOfTile, int gridX, int gridY)
    {
        // Enforce constraints on where we spawn tiles.
        if (gridX < 0 || gridX >= LevelGenerator.ROOM_WIDTH || gridY < 0 || gridY >= LevelGenerator.ROOM_HEIGHT)
        {
            throw new UnityException(string.Format("Attempted to spawn tile outside room boundaries. Tile: {0}, Grid X: {1}, Grid Y: {1}", tilePrefab, gridX, gridY));
        }

        GameObject tileObj = Instantiate(tilePrefab) as GameObject;
        tileObj.transform.parent = parentOfTile;
        AbbyTile tile = tileObj.GetComponent<AbbyTile>();
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
}
