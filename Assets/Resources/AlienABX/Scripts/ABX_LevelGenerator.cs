using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ABX_LevelGenerator : LevelGenerator
{
    [HideInInspector] public int maxRoomVal;
    [HideInInspector] public bool levelGenerationDone = false;
    void RestartLevelGen()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public override void generateCombinedRoomModeLevel()
    {
        levelGenerationDone = false;
        int currentRoomVal = 0;
        float totalRoomWidth = Tile.TILE_SIZE * ROOM_WIDTH;
        float totalRoomHeight = Tile.TILE_SIZE * ROOM_HEIGHT;

        Room[,] roomGrid = new Room[numXRooms, numYRooms];

        GameObject borderObjects = new GameObject("border_objects");
        borderObjects.transform.parent = GameManager.instance.transform;
        borderObjects.transform.localPosition = Vector3.zero;

        int currentRoomX = Random.Range(0, numXRooms);
        int currentRoomY = 0;
        List<Room> criticalPath = new List<Room>();


        Dir[] possibleDirsToPath = new Dir[] { Dir.Left, Dir.Left, Dir.Right, Dir.Right, Dir.Up };
        Dir currentDir = GlobalFuncs.randElem(possibleDirsToPath);
        Dir entranceDir = oppositeDir(currentDir);
        // Keep going in our current direction until we hit a will
        bool makingCriticalPath = true;
        GameObject roomToSpawn = startRoomPrefab;






        // This is based on Spelunky's method of building a critical path.
        // This code is kind of a mess and could likely be easily improved.=
        while (makingCriticalPath)
        {
            // Let's figure out what our required exits are going to be.
            Dir exitDir = Dir.Up;
            int nextRoomX = currentRoomX;
            int nextRoomY = currentRoomY;


            if (currentDir == Dir.Up)
            {
                if (currentRoomY >= numYRooms - 1)
                {
                    makingCriticalPath = false;
                }
                else
                {
                    exitDir = Dir.Up;
                    currentDir = GlobalFuncs.randElem(new Dir[] { Dir.Left, Dir.Right });
                    nextRoomY++;
                }
            }
            else if (currentDir == Dir.Left)
            {
                if (currentRoomX <= 0)
                {
                    if (currentRoomY >= numYRooms - 1)
                    {
                        makingCriticalPath = false;
                    }
                    else
                    {
                        exitDir = Dir.Up;
                        currentDir = Dir.Right;
                        nextRoomY++;
                    }
                }
                else
                {
                    // Move on if we randomly choose to
                    if (Random.Range(0, 5) == 0)
                    {
                        if (currentRoomY >= numYRooms - 1)
                        {
                            makingCriticalPath = false;
                        }
                        else
                        {
                            exitDir = Dir.Up;
                            nextRoomY++;
                        }
                    }
                    else
                    {
                        exitDir = Dir.Left;
                        nextRoomX--;
                    }
                }
            }
            else if (currentDir == Dir.Right)
            {
                if (currentRoomX >= numXRooms - 1)
                {
                    if (currentRoomY >= numYRooms - 1)
                    {
                        makingCriticalPath = false;
                    }
                    else
                    {
                        exitDir = Dir.Up;
                        currentDir = Dir.Left;
                        nextRoomY++;
                    }
                }
                else
                {
                    if (Random.Range(0, 5) == 0)
                    {
                        if (currentRoomY >= numYRooms - 1)
                        {
                            makingCriticalPath = false;
                        }
                        else
                        {
                            exitDir = Dir.Up;
                            nextRoomY++;
                        }
                    }
                    else
                    {
                        exitDir = Dir.Right;
                        nextRoomX++;
                    }
                }

            }


            if (!makingCriticalPath)
            {
                roomToSpawn = exitRoomPrefab;
            }

            Room room = null;
            ExitConstraint requiredExits = new ExitConstraint();
            if (roomToSpawn == startRoomPrefab)
            {
                requiredExits.addDirConstraint(exitDir);
                room = Room.generateRoom(roomToSpawn, this, currentRoomX, currentRoomY, 0, requiredExits);
                GameManager.instance.currentRoom = room;
            }
            else if (!makingCriticalPath)
            {
                requiredExits.addDirConstraint(entranceDir);
                room = Room.generateRoom(roomToSpawn, this, currentRoomX, currentRoomY, -1, requiredExits);
            }
            else
            {
                currentRoomVal++;
                requiredExits.addDirConstraint(entranceDir);
                requiredExits.addDirConstraint(exitDir);
                room = Room.generateRoom(roomToSpawn, this, currentRoomX, currentRoomY, currentRoomVal, requiredExits);
            }
            maxRoomVal = currentRoomVal;
            roomGrid[currentRoomX, currentRoomY] = room;
            criticalPath.Add(room);
            currentRoomX = nextRoomX;
            currentRoomY = nextRoomY;
            entranceDir = oppositeDir(exitDir);
            roomToSpawn = nextRoomToSpawn();
        }
        if (maxRoomVal < 8)
        {
            RestartLevelGen();
            return;
        }

        for (int x = 0; x < numXRooms; x++)
        {
            for (int y = 0; y < numYRooms; y++)
            {
                if (roomGrid[x, y] == null)
                {
                    roomGrid[x, y] = Room.generateRoom(nextRoomToSpawn(), this, x, y, -1, ExitConstraint.None);
                }
                float roomLeftX = totalRoomWidth * x - Tile.TILE_SIZE / 2;
                float roomRightX = totalRoomWidth * (x + 1) + Tile.TILE_SIZE / 2;
                float roomBottomY = totalRoomHeight * y - Tile.TILE_SIZE / 2;
                float roomTopY = totalRoomHeight * (y + 1) + Tile.TILE_SIZE / 2;

                if (x == 0 && y == 0)
                {
                    Vector2 bottomLeftWallGrid = Tile.toGridCoord(roomLeftX, roomBottomY);
                    spawnTileOutsideRoom(indestructibleWallPrefab, borderObjects.transform, (int)bottomLeftWallGrid.x, (int)bottomLeftWallGrid.y);
                }
                if (x == 0 && y == numYRooms - 1)
                {
                    Vector2 topLeftWallGrid = Tile.toGridCoord(roomLeftX, roomTopY);
                    spawnTileOutsideRoom(indestructibleWallPrefab, borderObjects.transform, (int)topLeftWallGrid.x, (int)topLeftWallGrid.y);
                }
                if (x == numXRooms - 1 && y == numYRooms - 1)
                {
                    Vector2 topRightWallGrid = Tile.toGridCoord(roomRightX, roomTopY);
                    spawnTileOutsideRoom(indestructibleWallPrefab, borderObjects.transform, (int)topRightWallGrid.x, (int)topRightWallGrid.y);
                }
                if (x == numXRooms - 1 && y == 0)
                {
                    Vector2 bottomRightWallGrid = Tile.toGridCoord(roomRightX, roomBottomY);
                    spawnTileOutsideRoom(indestructibleWallPrefab, borderObjects.transform, (int)bottomRightWallGrid.x, (int)bottomRightWallGrid.y);
                }

                if (x == 0)
                {
                    GameObject wallObj = Instantiate(verticalBorderWallPrefab) as GameObject;
                    wallObj.transform.parent = borderObjects.transform;
                    wallObj.transform.position = new Vector3(roomLeftX, (roomTopY + roomBottomY) / 2f, 0);
                }
                if (x == numXRooms - 1)
                {
                    GameObject wallObj = Instantiate(verticalBorderWallPrefab) as GameObject;
                    wallObj.transform.parent = borderObjects.transform;
                    wallObj.transform.position = new Vector3(roomRightX, (roomTopY + roomBottomY) / 2f, 0);
                }
                if (y == 0)
                {
                    GameObject wallObj = Instantiate(horizontalBorderWallPrefab) as GameObject;
                    wallObj.transform.parent = borderObjects.transform;
                    wallObj.transform.position = new Vector3((roomLeftX + roomRightX) / 2f, roomBottomY, 0);
                }
                if (y == numYRooms - 1)
                {
                    GameObject wallObj = Instantiate(horizontalBorderWallPrefab) as GameObject;
                    wallObj.transform.parent = borderObjects.transform;
                    wallObj.transform.position = new Vector3((roomLeftX + roomRightX) / 2f, roomTopY, 0);
                }
            }
        }


        GameManager.instance.roomGrid = roomGrid;
        GameManager.instance.borderObjects = borderObjects;




        ///<<Unimportant>>///
        // Now as a final step, if we're doing chaos mode, we need to randomly rearrange all spawned tiles (that aren't walls, players, or exits)
        if (GameManager.gameMode == GameManager.GameMode.Chaos)
        {
            List<Tile> tilesToRearrange = new List<Tile>();
            // Go through each room looking for tiles.
            for (int x = 0; x < numXRooms; x++)
            {
                for (int y = 0; y < numYRooms; y++)
                {
                    Room room = roomGrid[x, y];
                    foreach (Tile tile in room.GetComponentsInChildren<Tile>(true))
                    {
                        if (tile.hasTag(TileTags.Player | TileTags.Wall | TileTags.Exit))
                        {
                            continue;
                        }
                        tilesToRearrange.Add(tile);
                    }
                }
            }

            // Now we have a list of tiles, let's randomly shuffle their locations.
            for (int i = 0; i < tilesToRearrange.Count * 4; i++)
            {
                Tile tile1 = GlobalFuncs.randElem(tilesToRearrange);
                Tile tile2 = GlobalFuncs.randElem(tilesToRearrange);

                Transform tile1OldParent = tile1.transform.parent;
                Vector2 tile1OldPosition = tile1.transform.localPosition;

                tile1.transform.parent = tile2.transform.parent;
                tile1.transform.localPosition = new Vector3(tile2.transform.localPosition.x, tile2.transform.localPosition.y, tile1.transform.localPosition.z);

                tile2.transform.parent = tile1OldParent;
                tile2.transform.localPosition = new Vector3(tile1OldPosition.x, tile1OldPosition.y, tile2.transform.localPosition.z);

            }

        }


        levelGenerationDone = true;

    }
}
