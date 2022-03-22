using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//have your cursor over "room" to automatically go to it
public class ABX_Room : Room
{
    public TextMeshPro roomValTMP;
    public GameObject ABX_WallPrefab;
    public GameObject ABX_PlantPrefab;
    public GameObject ABX_DirtPrefab;
    public GameObject ABX_DoorPrefab;
    public GameObject ABX_UnbreakableWallPrefab;
    public GameObject ABX_ClayPrefab;
    public GameObject ABX_WoodPrefab;
    public GameObject ABX_ShopPrefab;

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
    ABX_LevelGenerator generator;
    public static int yoCount = 0;

    //move text_back in canvas to the top of the screen
    public void Start()
    {
        var textBack = GameObject.Find("Canvas").transform.Find("text_back").GetComponent<RectTransform>();
        var pos = textBack.anchoredPosition;
        textBack.anchoredPosition = new Vector3(pos.x, 260);        //change y to this number in Rect Transform of text_back
    }
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
        generator = (ABX_LevelGenerator)ourGenerator;
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
        StartCoroutine(WaitForGenerator());



    }
    IEnumerator WaitForGenerator()
    {
        while (!generator.levelGenerationDone)
        {
            yield return new WaitForEndOfFrame();
        }
        SpawnWalls();
        MakeOtherTiles();
        roomValTMP.text = roomVal.ToString();
        yield break;
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
        ///<<TODO>>
        // bool randomLeftExit = false, randomRightExit = false, randomUpExit = false, randomDownExit = false;
        // Additional random exits//
        // if (Random.Range(0f, 1f) < .5)
        //     randomDownExit = true;
        // if (Random.Range(0f, 1f) < .5)
        //     randomUpExit = true;
        // if (Random.Range(0f, 1f) < .5)
        //     randomLeftExit = true;
        // if (Random.Range(0f, 1f) < .5)
        //     randomRightExit = true;

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                if (_boolGrid[x, y] == false)
                    continue;
                if (_requiredExits.downExitRequired && (y == 0 || y == 1))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = -2;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.upExitRequired && (y == gridHeight - 1 || y == gridHeight - 2))
                    //leave a gap for a door
                    if ((x == 4 || x == 5))
                    {
                        _intGrid[x, y] = -1;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.leftExitRequired && (x == 0 || x == 1))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = -3;
                        _boolGrid[x, y] = false;
                        continue;
                    }

                if (_requiredExits.rightExitRequired && (x == gridWidth - 1 || x == gridWidth - 2))
                    //leave a gap for a door
                    if ((y == 3 || y == 4))
                    {
                        _intGrid[x, y] = -3;
                        _boolGrid[x, y] = false;
                        continue;
                    }


            }
    }
    void MakeOtherTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                //if nothing is in the tile
                if (_intGrid[x, y] == 0)
                {
                    // Plant
                    if (Random.Range(0f, 1f) < .5f)
                    {
                        _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_PlantPrefab, transform, x, y);
                        _intGrid[x, y] = 1;
                        _boolGrid[x, y] = true;
                    }

                    //spawning the shop
                    if (_requiredExits.upExitRequired)
                    {
                        // if (Random.Range(0f, 1f) < .3f && !shopSpawned)
                        // {
                        //     _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_ShopPrefab, transform, x, y);
                        //     _intGrid[x, y] = 1;
                        //     _boolGrid[x, y] = true;
                        //     shopSpawned = true;
                        //     continue;
                        // }
                    }
                }

                //if something required in each room hasn't spawned
                if (x == gridWidth - 1 && y == gridHeight - 1)
                {


                }
            }
        }

        //Below here is stuff that is spawned more sparingly
        bool firstShopSpawned = false;
        bool secondShopSpawned = false;
        bool thirdShopSpawned = false;

        ///<<SHOP>>///
        if (_requiredExits.upExitRequired && !firstShopSpawned && roomGridY == 1)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_ShopPrefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;
            firstShopSpawned = true;
        }

        if (_requiredExits.upExitRequired && !secondShopSpawned && roomGridY == 2)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_ShopPrefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;
            secondShopSpawned = true;
        }

        if (roomVal == generator.maxRoomVal && !thirdShopSpawned)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_ShopPrefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;
            thirdShopSpawned = true;
        }
        ///<<SHOP>><<END>>


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
        bool firstDoorSpawned = false;
        bool secondDoorSpawned = false;

        // Record ground vals and assign colors
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                ClearWall(x, y);
                _tileGrid[x, y] = null;

                //First unbreakable wall
                if (roomGridY == 1 && y == gridHeight - 1)
                {
                    // spawning door
                    if (_intGrid[x, y] == -1 && !firstDoorSpawned)
                    {
                        _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_DoorPrefab, transform, x, y);
                        firstDoorSpawned = true;
                        continue;
                    }

                    _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_UnbreakableWallPrefab, transform, x, y);
                    continue;
                }
                //Second unbreakable wall
                if (roomGridY == 3 && y == 0)
                {
                    //spawning door
                    if (_intGrid[x, y] == -2 && !secondDoorSpawned)
                    {
                        _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_DoorPrefab, transform, x, y);
                        secondDoorSpawned = true;
                        continue;
                    }
                    _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_UnbreakableWallPrefab, transform, x, y);
                    continue;
                }


                if (_boolGrid[x, y])
                {

                    //Dirt walls
                    if (_intGrid[x, y] == 1 ||
                        _intGrid[x, y] == 2 ||
                        _intGrid[x, y] == 3 ||
                        _intGrid[x, y] == 4 ||
                        _intGrid[x, y] == 5 ||
                        _intGrid[x, y] == 6 ||
                        _intGrid[x, y] == 7)
                    {
                        // _tileGrid[x, y] = ABX_Tile.spawnABX_Tile(ABX_DirtPrefab, transform, x, y);
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