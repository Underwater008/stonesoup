using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//have your cursor over "room" to automatically go to it
public class ABX_Room : Room
{
    public GameObject ABX_WallPrefab;
    public GameObject ABX_PlantPrefab;
    public GameObject ABX_DirtPrefab;
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
        _tileGrid = new ABX_Tile[gridWidth, gridHeight];


        WallBools();
        RandomWalls();
        SetGroundVals();
        CreateCave();
        MakeExits();
        MakeAlcloves();

        SpawnWalls();
        MakeOtherTiles();


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
    void MakeAlcloves()
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

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                if (_boolGrid[x, y] == false)
                    continue;

                if (randomAlclove)
                    if (x >= alcloveXMin && x <= alcloveXMax)
                        if (y >= alcloveYMin && y <= alcloveYMax)
                        {
                            _intGrid[x, y] = 0;
                            _boolGrid[x, y] = false;
                            continue;
                        }



            }
    }
    void MakeExits()
    {
        //Additional random exits//
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
                if (_boolGrid[x, y] == false)
                    continue;
                if (_requiredExits.downExitRequired && (y == 0 || y == 1))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = 0;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.upExitRequired && (y == gridHeight - 1 || y == gridHeight - 2))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = 0;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.leftExitRequired && (x == 0 || x == 1))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = 0;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.rightExitRequired && (x == gridWidth - 1 || x == gridWidth - 2))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = 0;
                        _boolGrid[x, y] = false;
                        continue;
                    }


            }
    }
    void MakeOtherTiles()
    {

        bool hasOmnisphere = false;
        bool omniSphereSpawned = false;
        if (Random.Range(0f, 1f) < .3)
        {
            hasOmnisphere = true;
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (omniSphereSpawned)
                    continue;

                if (_intGrid[x, y] == 0)
                {
                    if (Random.Range(0f, 1f) < .5f)
                    {
                        _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_PlantPrefab, transform, x, y);
                    }

                }
            }
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

                    if (_intGrid[x, y] == 1 ||
                        _intGrid[x, y] == 2 ||
                        _intGrid[x, y] == 3 ||
                        _intGrid[x, y] == 4 ||
                        _intGrid[x, y] == 5 ||
                        _intGrid[x, y] == 6)
                    {
                        _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_DirtPrefab, transform, x, y);
                        continue;
                    }
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




}