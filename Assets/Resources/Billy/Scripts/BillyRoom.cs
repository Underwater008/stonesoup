using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillyRoom : Room
{
    bool[,] _wallGrid = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
    bool[,] _nextWallGrid = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];

    [Header("CA Parameters")]
    public int neighborLowerLimit;
    public int neighborUpperLimit;
    public int reviveLimit;
    public float startingChance;
    public int iterations;

    [Header("Digger Parameters")]
    public int minBranchLength;
    public int maxBranchLength;
    public int minPathLength;
    public int maxPathLength;


    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {
        //CA Generation

        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            requiredExits.downExitRequired = true;
        }
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            requiredExits.upExitRequired = true;
        }
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            requiredExits.leftExitRequired = true;
        }
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            requiredExits.rightExitRequired = true;
        }
        FillRoomCA(requiredExits);
        


        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                if (!_wallGrid[x, y])
                {
                    SpawnTile(x, y);
                    continue;
                }
                Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
            }
        }
    }

    //Just a placeholder function to spawn things
    void SpawnTile (int x, int y)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random < 0.02f)
        {
            Tile.spawnTile(localTilePrefabs[0], transform, x, y);
        }
        else if (random < 0.07f)
        {
            Tile.spawnTile(localTilePrefabs[1], transform, x, y);
        }
        else if (random < 0.10f)
        {
            Tile.spawnTile(localTilePrefabs[2], transform, x, y);
        }
        else if (random < 0.11f)
        {
            Tile.spawnTile(localTilePrefabs[3], transform, x, y);
        }
    }

    bool NextCAValue(int x, int y)
    {
        int neighborCount = 0;
        if (x == 0 || x == (_wallGrid.GetLength(0) - 1))
        {
            return true;
        }
        if (y == 0 || y == (_wallGrid.GetLength(1) - 1))
        {
            return true;
        }

        for (int r = -1; r < 2; r++)
        {
            for (int c = -1; c < 2; c++)
            {
                if (r == 0 && c == 0)
                {
                    continue;
                }
                if (_wallGrid[x + r, y + c])
                {
                    neighborCount++;
                }
            }
        }

        if (neighborCount > neighborUpperLimit || neighborCount < neighborLowerLimit)
        {
            return false;
        }
        if (!_wallGrid[x, y])
        {
            if (neighborCount < reviveLimit)
            {
                return false;
            }
        }
        return true;

    }

    void PerformCAStep()
    {
        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                _nextWallGrid[x, y] = NextCAValue(x, y);
            }
        }

        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                _wallGrid[x, y] = _nextWallGrid[x, y];
            }
        }
    }

    void FillRoomCA (ExitConstraint exitReq)
    {
        for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
        {
            for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
            {
                if (Random.Range(0.0f, 1.0f) < startingChance)
                {
                    _wallGrid[x, y] = true;
                }
            }
        }
        for (int i = 0; i < iterations; i++)
        {
            PerformCAStep();
        }

        Vector2Int exitOne = new Vector2Int(-1, -1);
        Vector2Int exitTwo = new Vector2Int(-1, -1);


        if (exitReq.downExitRequired)
        {
            int exit = Random.Range(1, LevelGenerator.ROOM_WIDTH - 2);
            _wallGrid[exit, 0] = false;
            _wallGrid[exit + 1, 0] = false;
            if (exitOne.x == -1)
            {
                exitOne = new Vector2Int(exit, 0);
            }
            else
            {
                exitTwo = new Vector2Int(exit, 0);
            }
        }
        if (exitReq.upExitRequired)
        {
            int exit = Random.Range(1, LevelGenerator.ROOM_WIDTH - 2);
            _wallGrid[exit, LevelGenerator.ROOM_HEIGHT - 1] = false;
            _wallGrid[exit + 1, LevelGenerator.ROOM_HEIGHT - 1] = false;
            if (exitOne.x == -1)
            {
                exitOne = new Vector2Int(exit, LevelGenerator.ROOM_HEIGHT - 1);
            }
            else
            {
                exitTwo = new Vector2Int(exit, LevelGenerator.ROOM_HEIGHT - 1);
            }
        }
        if (exitReq.leftExitRequired)
        {
            int exit = Random.Range(1, LevelGenerator.ROOM_HEIGHT - 2);
            _wallGrid[0, exit] = false;
            _wallGrid[0, exit + 1] = false;
            if (exitOne.x == -1)
            {
                exitOne = new Vector2Int(0, exit);
            }
            else
            {
                exitTwo = new Vector2Int(0, exit);
            }
        }
        if (exitReq.rightExitRequired)
        {
            int exit = Random.Range(1, LevelGenerator.ROOM_HEIGHT - 2);
            _wallGrid[LevelGenerator.ROOM_WIDTH - 1, exit] = false;
            _wallGrid[LevelGenerator.ROOM_WIDTH - 1, exit + 1] = false;
            if (exitOne.x == -1)
            {
                exitOne = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, exit);
            }
            else
            {
                exitTwo = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, exit);
            }
        }
        DigPath(exitOne, exitTwo);
    }

    void FillRoomDigger ()
    {
        
    }

    void DigPath (Vector2Int start, Vector2Int end)
    {
        if (start.x < 0 || start.y < 0 || end.x < 0 || end.y < 0)
        {
            //Debug.Log("Dig Path: End Points out of bounds");
            return;
        }
        Vector2Int _current = start;
        float _xDiffStart = end.x - start.x;
        float _yDiffStart = end.y - start.y;
        int _xDir = Mathf.RoundToInt(Mathf.Sign(_xDiffStart));
        int _yDir = Mathf.RoundToInt(Mathf.Sign(_yDiffStart));

        //See if we only need to dig in one direction
        if (end.x == start.x)
        {
            while (!_current.Equals(end))
            {
                _wallGrid[_current.x, _current.y] = false;
                _current = new Vector2Int(_current.x, _current.y + _yDir);
            }
            _wallGrid[_current.x, _current.y] = false;
            return;
        }
        else if (end.y == start.y)
        {
            while (!_current.Equals(end))
            {
                _wallGrid[_current.x, _current.y] = false;
                _current = new Vector2Int(_current.x + _xDir, _current.x);
            }
            _wallGrid[_current.x, _current.y] = false;
            return;
        }

        //Dig in two directions and maintain the general angle between the start and the end
        while (!_current.Equals(end))
        {
            _wallGrid[_current.x, _current.y] = false;

            float _xDiff = end.x - _current.x;
            float _yDiff = end.y - _current.y;
            
            if (_xDiff / _xDiffStart > _yDiff / _yDiffStart)
            {
                _current = new Vector2Int(_current.x + _xDir, _current.y);
            }
            else
            {
                _current = new Vector2Int(_current.x, _current.y + _yDir);
            }
        }
        _wallGrid[_current.x, _current.y] = false;
    }
}
