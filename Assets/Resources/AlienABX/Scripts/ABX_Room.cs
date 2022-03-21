using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//have your cursor over "room" to automatically go to it
public class ABX_Room : Room
{
    public GameObject ABX_WallPrefab;
    List<Color> wallColors = new List<Color>();
    bool[,] _boolGrid, _nextBoolGrid;
    [HideInInspector] public int[,] _intGrid;
    ABX_Tile[,] _tileGrid;
    int gridWidth, gridHeight;
    public int automataSteps = 5;
    public int caDeathLimit = 4;
    public int caBirthLimit = 5;
    public float spawnChance;

    ExitConstraint _requiredExits;
    public static int yoCount = 0;
    void AssignWallColors()
    {
        wallColors.Clear();
        for (int i = 0; i < 11; i++)
        {
            Color c = new Color(i / 10f, i / 10f, i / 10f, 1);
            wallColors.Add(c);
        }
    }
    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits)
    {

        AssignWallColors();
        _requiredExits = requiredExits;
        gridWidth = LevelGenerator.ROOM_WIDTH;
        gridHeight = LevelGenerator.ROOM_HEIGHT;
        _boolGrid = new bool[gridWidth, gridHeight];
        _nextBoolGrid = new bool[gridWidth, gridHeight];
        _intGrid = new int[gridWidth, gridHeight];
        _tileGrid = new ABX_Wall[gridWidth, gridHeight];


        WallBools();
        RandomWalls();
        MakeExits();
        SetGroundVals();
        CreateCave();

        Debug.Log(MeetsConstraints(requiredExits));
        // Debug.Log(_requiredExits.downExitRequired);
        if (!MeetsConstraints(_requiredExits))
        {
            yoCount++;
            Debug.Log(yoCount);
            DeleteWalls();
            WallBools();
            RandomWalls();
            MakeExits();
            SetGroundVals();
            CreateCave();
        }


        SpawnWalls();

        MakeOtherTiles();

    }
    void MakeExits()
    {

        //Alclove//
        bool randomAlclove = false;
        int alcloveXMin = 0, alcloveXMax = 0, alcloveYMin = 0, alcloveYMax = 0;
        if (Random.Range(0f, 1f) < .2)
        {
            randomAlclove = true;
            alcloveXMin += Random.Range(0, gridWidth);
            alcloveXMax += Random.Range(alcloveXMin, gridWidth);
            alcloveYMin += Random.Range(0, gridHeight);
            alcloveYMax += Random.Range(alcloveYMin, gridHeight);

        }


        //Additional exits//
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.downExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.upExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.leftExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.rightExitRequired = true;



        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                if (_tileGrid[x, y] == null)
                    continue;
                if (_requiredExits.downExitRequired && (y == 0 || y == 1))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = 0;
                        continue;
                    }

                if (_requiredExits.upExitRequired && (y == gridHeight - 1 || y == gridHeight - 2))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = 0;
                        continue;
                    }

                if (_requiredExits.leftExitRequired && (x == 0 || x == 1))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = 0;
                        continue;
                    }

                if (_requiredExits.rightExitRequired && (x == gridWidth - 1 || x == gridWidth - 2))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = 0;
                        continue;
                    }

                if (randomAlclove)
                    if (x >= alcloveXMin && x <= alcloveXMax)
                        if (y >= alcloveYMin && y <= alcloveYMax)
                        {
                            _intGrid[x, y] = 0;
                            continue;
                        }


            }
    }



    void DeleteWalls()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                ClearWall(x, y);
            }
        }

    }
    void ClearWall(int x, int y)
    {
        if (_tileGrid[x, y] != null)
            _tileGrid[x, y].remove();
    }

    void MakeOtherTiles()
    {

        bool randomAlclove = false;
        int alcloveXMin = 0, alcloveXMax = 0, alcloveYMin = 0, alcloveYMax = 0;
        if (Random.Range(0f, 1f) < .2)
        {
            randomAlclove = true;
            alcloveXMin += Random.Range(0, gridWidth);
            alcloveXMax += Random.Range(alcloveXMin, gridWidth);
            alcloveYMin += Random.Range(0, gridHeight);
            alcloveYMax += Random.Range(alcloveYMin, gridHeight);

        }

        bool hasRequiredExits = false;

        if (_requiredExits.downExitRequired || _requiredExits.upExitRequired || _requiredExits.leftExitRequired || _requiredExits.rightExitRequired)
            hasRequiredExits = true;

        bool hasOmnisphere = false;
        if (Random.Range(0f, 1f) < .3)
        {
            hasOmnisphere = true;
        }
        //Additional exits//
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.downExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.upExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.leftExitRequired = true;
        if (Random.Range(0f, 1f) < .5)
            _requiredExits.rightExitRequired = true;


        bool hasItem = false;
        bool itemSpawned = false;
        if (hasRequiredExits)
        {
            if (Random.Range(0f, 1f) < .9f)
            {
                hasItem = true;
            }
        }

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                if (_tileGrid[x, y] == null)
                    continue;
                if (_requiredExits.downExitRequired && (y == 0 || y == 1))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _tileGrid[x, y].remove();
                        continue;
                    }

                if (_requiredExits.upExitRequired && (y == gridHeight - 1 || y == gridHeight - 2))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _tileGrid[x, y].remove();
                        continue;
                    }

                if (_requiredExits.leftExitRequired && (x == 0 || x == 1))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _tileGrid[x, y].remove();
                        continue;
                    }

                if (_requiredExits.rightExitRequired && (x == gridWidth - 1 || x == gridWidth - 2))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _tileGrid[x, y].remove();
                        continue;
                    }

                if (randomAlclove)
                    if (x >= alcloveXMin && x <= alcloveXMax)
                        if (y >= alcloveYMin && y <= alcloveYMax)
                        {
                            _tileGrid[x, y].remove();
                            if (hasItem && !itemSpawned)
                            {
                                if (Random.Range(0f, 1f) < 1f)
                                {
                                    //Always getting a null ref here
                                    // _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(omnispherePrefab, transform, x, y);
                                    itemSpawned = true;
                                    continue;
                                }
                            }
                            continue;
                        }

                // if (hasOmnisphere)
                // {
                //     if (x == 5 && y == 5)
                //     {
                //         _tileGrid[x, y].remove();
                //         _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(localTilePrefabs[1], this.transform, x, y);
                //     }
                // }


            }
    }

    void WallBools()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //first set them all to false
                _boolGrid[x, y] = false;

                if (x != 0 && x != LevelGenerator.ROOM_WIDTH - 1 &&
                    y != 0 && y != LevelGenerator.ROOM_HEIGHT - 1)
                    continue; //continue goes to the next iteration of the loop

                _boolGrid[x, y] = true;
            }
        }

    }

    void RandomWalls()
    {
        //Spawn the extra walls//
        for (int i = 0; i < Mathf.FloorToInt(spawnChance * gridWidth * gridHeight); i++)
        {
            int r1 = Random.Range(1, gridWidth - 1);
            int r2 = Random.Range(1, gridHeight - 1);
            if (!_boolGrid[r1, r2])
                _boolGrid[r1, r2] = true;
            else
                i--;
        }

    }
    void SetGroundVals()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                if (_boolGrid[x, y])
                {
                    int groundVal = CountAliveNeighbors(_boolGrid, x, y);
                    _intGrid[x, y] = groundVal;
                }
            }
    }
    void SpawnWalls()
    {
        // Record ground vals and assign colors
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                ClearWall(x, y);
                _tileGrid[x, y] = null;
                if (_boolGrid[x, y])
                {

                    _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_WallPrefab, transform, x, y);
                    _tileGrid[x, y].GetComponentInChildren<SpriteRenderer>().color = wallColors[_intGrid[x, y]];
                }
            }
    }


    int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                int neighbor_x = x + i;
                int neighbor_y = y + j;

                //If not at middle point
                if (i != 0 || j != 0)
                {
                    // Index at edge of map
                    if (neighbor_x < 0 || neighbor_y < 0 || neighbor_x >= grid.GetLength(0) || neighbor_y >= grid.GetLength(1))
                        count++;
                    else if (grid[neighbor_x, neighbor_y])
                        count++;
                }
            }
        return count;
    }
    public override Room createRoom(ExitConstraint requiredExits)
    {
        GameObject roomObj = Instantiate(gameObject);
        return roomObj.GetComponent<ABX_Room>();
    }
    void CreateCave()
    {
        AutomateToCave(automataSteps, _boolGrid, _nextBoolGrid, this.transform, _intGrid);
    }

    void AutomateToCave(int steps, bool[,] grid, bool[,] nextGrid, Transform gridParent, int[,] intGrid)
    {
        for (int i = 0; i < steps; i++)
        {
            PerformCAStep();
            RespawnWalls();
        }
    }
    void RespawnWalls()
    {
        PerformCAStep();
        SetGroundVals();
    }
    void PerformCAStep()
    {

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                _nextBoolGrid[x, y] = NextCAValue(x, y, _boolGrid);

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                _boolGrid[x, y] = _nextBoolGrid[x, y];
    }

    bool NextCAValue(int x, int y, bool[,] grid)
    {
        bool newVal = false;
        bool[,] tempGrid = grid;

        int nbs = CountAliveNeighbors(tempGrid, x, y);
        if (tempGrid[x, y])
        {
            if (nbs < caDeathLimit)
                newVal = false;
            else
                newVal = true;
        }
        else
        {
            if (nbs > caBirthLimit)
                newVal = true;
            else
                newVal = false;
        }
        //so it'll never remove any sides
        if (x >= gridWidth - 1 || x <= 0 || y >= gridHeight - 1 || y <= 0)
            newVal = true;

        return newVal;
    }



    bool _hasUpExit, _hasDownExit, _hasLeftExit, _hasRightExit;
    bool _hasUpLeftPath, _hasUpRightPath, _hasUpDownPath, _hasRightDownPath, _hasRightLeftPath, _hasDownLeftPath;

    void ResetBools()
    {
        _hasUpExit = false;
        _hasDownExit = false;
        _hasLeftExit = false;
        _hasRightExit = false;

        _hasUpLeftPath = false;
        _hasUpRightPath = false;
        _hasUpDownPath = false;
        _hasRightDownPath = false;
        _hasRightLeftPath = false;
        _hasDownLeftPath = false;
    }

    private void ValidateRoom()
    {
        // Debug.Log(LevelGenerator.ROOM_WIDTH);
        Vector2Int upExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, LevelGenerator.ROOM_HEIGHT - 1);
        Vector2Int downExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, 0);
        Vector2Int leftExit = new Vector2Int(0, LevelGenerator.ROOM_HEIGHT / 2);
        Vector2Int rightExit = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, LevelGenerator.ROOM_HEIGHT / 2);
        _hasUpExit = IsPointNavigable(upExit);
        _hasDownExit = IsPointNavigable(downExit);
        _hasLeftExit = IsPointNavigable(leftExit);
        _hasRightExit = IsPointNavigable(rightExit);
        _hasUpLeftPath = DoesPathExist(upExit, leftExit);
        _hasUpRightPath = DoesPathExist(upExit, rightExit);
        _hasUpDownPath = DoesPathExist(upExit, downExit);
        _hasRightDownPath = DoesPathExist(rightExit, downExit);
        _hasRightLeftPath = DoesPathExist(rightExit, leftExit);
        _hasDownLeftPath = DoesPathExist(downExit, leftExit);
        // Debug.Log("_hasUpexit: " + _hasUpExit);
        // Debug.Log("_hasDownExit: " + _hasDownExit);
        // Debug.Log("_hasLeftExit: " + _hasLeftExit);
        // Debug.Log("_hasRightExit: " + _hasRightExit);
        // Debug.Log("_hasUpLeftPath: " + _hasUpLeftPath);
        // Debug.Log("_hasUpRightPath: " + _hasUpRightPath);
        // Debug.Log("_hasUpDownPath: " + _hasUpDownPath);
        // Debug.Log("_hasRightDownPath: " + _hasRightDownPath);
        // Debug.Log("_hasRightLeftPath: " + _hasRightLeftPath);
        // Debug.Log("_hasDownLeftPath: " + _hasDownLeftPath);
    }

    public bool MeetsConstraints(ExitConstraint requiredExits)
    {
        ValidateRoom();
        if (requiredExits.upExitRequired && _hasUpExit == false)
            return false;
        if (requiredExits.downExitRequired && _hasDownExit == false)
            return false;
        if (requiredExits.leftExitRequired && _hasLeftExit == false)
            return false;
        if (requiredExits.rightExitRequired && _hasRightExit == false)
            return false;
        if (_hasUpLeftPath == false && requiredExits.upExitRequired && requiredExits.leftExitRequired)
            return false;
        if (_hasUpRightPath == false && requiredExits.upExitRequired && requiredExits.rightExitRequired)
            return false;
        if (_hasUpDownPath == false && requiredExits.upExitRequired && requiredExits.downExitRequired)
            return false;
        if (_hasRightDownPath == false && requiredExits.rightExitRequired && requiredExits.downExitRequired)
            return false;
        if (_hasRightLeftPath == false && requiredExits.rightExitRequired && requiredExits.leftExitRequired)
            return false;
        if (_hasDownLeftPath == false && requiredExits.downExitRequired && requiredExits.leftExitRequired)
            return false;
        return true;
    }
    bool IsPointInGrid(Vector2Int point)
    {
        if (point.x < 0 || point.x >= LevelGenerator.ROOM_WIDTH)
            return false;
        if (point.y < 0 || point.y >= LevelGenerator.ROOM_HEIGHT)
            return false;
        return true;
    }
    bool IsPointNavigable(Vector2Int point)
    {
        if (IsPointInGrid(point) == false)
            return false;
        if (_intGrid[point.x, point.y] == 1)
            return false;

        else
            return true;
    }
    bool DoesPathExist(Vector2Int startPoint, Vector2Int endPoint)
    {
        List<Vector2Int> openSet = new List<Vector2Int>();
        List<Vector2Int> closedSet = new List<Vector2Int>();
        if (IsPointNavigable(startPoint))
        {
            openSet.Add(startPoint);
        }

        while (openSet.Count > 0)
        {
            Vector2Int currentPoint = openSet[0];
            openSet.RemoveAt(0);
            if (currentPoint == endPoint)
                return true;
            //up neighbor
            Vector2Int upNeighbor = new Vector2Int(currentPoint.x, currentPoint.y + 1);
            if (openSet.Contains(upNeighbor) == false && closedSet.Contains(upNeighbor) == false)
            {
                if (IsPointNavigable(upNeighbor))
                    openSet.Add(upNeighbor);
            }
            //down neighbor
            Vector2Int downNeighbor = new Vector2Int(currentPoint.x, currentPoint.y - 1);
            if (openSet.Contains(downNeighbor) == false && closedSet.Contains(downNeighbor) == false)
            {
                if (IsPointNavigable(downNeighbor))
                    openSet.Add(downNeighbor);
            }
            //left neighbor
            Vector2Int leftNeighbor = new Vector2Int(currentPoint.x - 1, currentPoint.y);
            if (openSet.Contains(leftNeighbor) == false && closedSet.Contains(leftNeighbor) == false)
            {
                if (IsPointNavigable(leftNeighbor))
                    openSet.Add(leftNeighbor);
            }
            //right neighbor
            Vector2Int rightNeighbor = new Vector2Int(currentPoint.x + 1, currentPoint.y);
            if (openSet.Contains(rightNeighbor) == false && closedSet.Contains(rightNeighbor) == false)
            {
                if (IsPointNavigable(rightNeighbor))
                    openSet.Add(rightNeighbor);
            }
            closedSet.Add(currentPoint);
        }
        return false;
    }


}