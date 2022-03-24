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
    public GameObject ABX_RS1Prefab;
    public GameObject ABX_RS2Prefab;
    public GameObject ABX_RS3Prefab;
    public GameObject ABX_ShearPrefab;
    public GameObject ABX_ShovelPrefab;
    public GameObject ABX_GasMaskPrefab;
    public GameObject ABX_AlienPrefab;



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
        // textBack.anchoredPosition = new Vector3(pos.x, 0);        //change y to this number in Rect Transform of text_back
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

        // Debug.Log(MeetsConstraints(requiredExits));
        while (!MeetsConstraints(requiredExits))
        {
            DeleteWalls();
            WallBools();
            RandomWalls();
            SetGroundVals();
            CreateCave();
        }

        MakeExits();
        // MakeAlcloves();
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
                _boolGrid[x, y] = false;
                _intGrid[x, y] = 0;
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
        //make it : 
        //so the critical path cant be short

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


        ///<<RESEARCH>><<STATIONS>>///
        if (_requiredExits.upExitRequired && !firstShopSpawned && roomGridY == 1)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_RS1Prefab, transform, x1, y1);
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
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_RS2Prefab, transform, x1, y1);
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
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_RS3Prefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;
            thirdShopSpawned = true;
        }

        ///<<RESEARCH>><<END>>///
        ///<<SHOVEL>>///
        bool shovelSpawned = false;

        if (!shovelSpawned && roomVal == 1)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_ShovelPrefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;


            shovelSpawned = true;
        }
        ///<<SHEAR>>///
        bool shearSpawned = false;
        //The shear
        if (_requiredExits.downExitRequired && !shearSpawned && roomGridY == 2)
        {
            int x1 = 0;
            int y1 = 0;
            while (_intGrid[x1, y1] != 0)
            {
                x1 = Random.Range(0, gridWidth);
                y1 = Random.Range(0, gridHeight);
            }
            _tileGrid[x1, y1] = ABX_Tile.spawnABX_Tile(ABX_ShearPrefab, transform, x1, y1);
            _intGrid[x1, y1] = 1;
            _boolGrid[x1, y1] = true;


            shearSpawned = true;
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
    //TODO: Room validation
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
    ///<<Validation>>///

    public bool _hasUpExit;
    public bool _hasDownExit;
    public bool _hasLeftExit;
    public bool _hasRightExit;

    public bool _hasUpLeftPath;
    public bool _hasUpRightPath;
    public bool _hasUpDownPath;
    public bool _hasRightDownPath;
    public bool _hasRightLeftPath;
    public bool _hasDownLeftPath;
    private void ValidateRoom()
    {
        int[,] indexGrid = _intGrid;
        Vector2Int upExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, LevelGenerator.ROOM_HEIGHT - 1);
        Vector2Int downExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, 0);
        Vector2Int leftExit = new Vector2Int(0, LevelGenerator.ROOM_HEIGHT / 2);
        Vector2Int rightExit = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, LevelGenerator.ROOM_HEIGHT / 2);
        _hasUpExit = IsPointNavigable(indexGrid, upExit);
        _hasDownExit = IsPointNavigable(indexGrid, downExit);
        _hasLeftExit = IsPointNavigable(indexGrid, leftExit);
        _hasRightExit = IsPointNavigable(indexGrid, rightExit);
        _hasUpLeftPath = DoesPathExist(indexGrid, upExit, leftExit);
        _hasUpRightPath = DoesPathExist(indexGrid, upExit, rightExit);
        _hasUpDownPath = DoesPathExist(indexGrid, upExit, downExit);
        _hasRightDownPath = DoesPathExist(indexGrid, rightExit, downExit);
        _hasRightLeftPath = DoesPathExist(indexGrid, rightExit, leftExit);
        _hasDownLeftPath = DoesPathExist(indexGrid, downExit, leftExit);
        // Debug.Log("Room: " + roomVal);
        // Debug.Log("hasUpexit: " + _hasUpExit);
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



    bool DoesPathExist(int[,] indexGrid, Vector2Int startPoint, Vector2Int endPoint)
    {
        List<Vector2Int> openSet = new List<Vector2Int>();
        List<Vector2Int> closedSet = new List<Vector2Int>();
        if (IsPointNavigable(indexGrid, startPoint))
            openSet.Add(startPoint);
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
                if (IsPointNavigable(indexGrid, upNeighbor))
                    openSet.Add(upNeighbor);
            }
            //down neighbor
            Vector2Int downNeighbor = new Vector2Int(currentPoint.x, currentPoint.y - 1);
            if (openSet.Contains(downNeighbor) == false && closedSet.Contains(downNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, downNeighbor))
                    openSet.Add(downNeighbor);
            }
            //left neighbor
            Vector2Int leftNeighbor = new Vector2Int(currentPoint.x - 1, currentPoint.y);
            if (openSet.Contains(leftNeighbor) == false && closedSet.Contains(leftNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, leftNeighbor))
                    openSet.Add(leftNeighbor);
            }
            //right neighbor
            Vector2Int rightNeighbor = new Vector2Int(currentPoint.x + 1, currentPoint.y);
            if (openSet.Contains(rightNeighbor) == false && closedSet.Contains(rightNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, rightNeighbor))
                    openSet.Add(rightNeighbor);
            }
            closedSet.Add(currentPoint);
        }
        return false;
    }

    bool IsPointNavigable(int[,] indexGrid, Vector2Int point)
    {

        if (IsPointInGrid(point) == false)
            return false;
        if (roomVal == 1 && indexGrid[point.x, point.y] < 6)
        {
            return true;
        }
        if (roomVal != 1 && indexGrid[point.x, point.y] < 7)
            return true;
        else
            return false;

    }


    bool IsPointInGrid(Vector2Int point)
    {
        if (point.x < 0 || point.x >= LevelGenerator.ROOM_WIDTH)
            return false;
        if (point.y < 0 || point.y >= LevelGenerator.ROOM_HEIGHT)
            return false;
        return true;
    }

}