using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23LockedRoomRandom : Room
{
    public GameObject doorPrefab;
	public GameObject enemyPrefab;
	public GameObject enemy2Prefab;
    public GameObject healPrefab;
    public GameObject keyPrefab;

    //public int minNumDoor = 4, maxNumDoor = 4;
	public int minNumEnemies = 1, maxNumEnemies = 3;
    public int minNumHeals = 1, maxNumHeals = 1;
    public int minNumKeys = 1, maxNumKeys = 3;

    public float borderWallProbability = 1f;

    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// It's very likely you'll want to do different generation methods depending on which required exits you receive
		// Here's an example of randomly choosing between two generation methods.
		float random = Random.value;
		if (random <= 0.2f) {
			
			roomGenerationVersionOne(ourGenerator, requiredExits);
		}		
		else if(random <=0.4f){
			roomGenerationVersionTwo(ourGenerator, requiredExits);
		}
		else if(random <= 0.6f)
        {
			roomGenerationVersionThree(ourGenerator, requiredExits);
		}
		else if(random <=0.8f)
        {
			roomGenerationVersionFour(ourGenerator, requiredExits);
		}
        else
        {
			roomGenerationVersionFive(ourGenerator, requiredExits);
		}

	}



    protected void roomGenerationVersionOne(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// In this version of room generation, I only generate the walls.
		generateWalls(ourGenerator, requiredExits);
		generateDoor(ourGenerator);
	
		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else {
					occupiedPositions[x, y] = false;
				}
			}
		}

	}

     protected void roomGenerationVersionTwo(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// In this version of room generation, I only generate the walls.
		generateWalls(ourGenerator, requiredExits);
		generateDoor(ourGenerator);

		int numKeys = Random.Range(minNumKeys, maxNumKeys+1);
		int numEnemies = Random.Range(minNumEnemies, maxNumEnemies+1);

		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else {
					occupiedPositions[x, y] = false;
				}
			}
		}

				// Now we spawn rocks and enemies in random locations
		List<Vector2> possibleSpawnPositions = new List<Vector2>(LevelGenerator.ROOM_WIDTH*LevelGenerator.ROOM_HEIGHT);
		for (int i = 0; i < numKeys; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 1; x < LevelGenerator.ROOM_WIDTH -1; x++) {
				for (int y = 1; y < LevelGenerator.ROOM_HEIGHT -1; y++) {
					if (occupiedPositions[x, y]) {
						continue;
					}
					possibleSpawnPositions.Add(new Vector2(x, y));
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				Tile.spawnTile(keyPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}
		for (int i = 0; i < numEnemies; i++) {
			possibleSpawnPositions.Clear();
			for (int x = 1; x < LevelGenerator.ROOM_WIDTH -1; x++) {
				for (int y = 1; y < LevelGenerator.ROOM_HEIGHT -1; y++) {
					if (occupiedPositions[x, y]) {
						continue;
					}
					possibleSpawnPositions.Add(new Vector2(x, y));
				}
			}
			if (possibleSpawnPositions.Count > 0) {
				Vector2 spawnPos = GlobalFuncs.randElem(possibleSpawnPositions);
				Tile.spawnTile(enemyPrefab, transform, (int)spawnPos.x, (int)spawnPos.y);
				occupiedPositions[(int)spawnPos.x, (int)spawnPos.y] = true;
			}
		}
	}

	protected void roomGenerationVersionThree(LevelGenerator ourGenerator, ExitConstraint requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);
		generateDoor(ourGenerator);

		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
		{
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
			{
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH - 1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1)
				{
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else
				{
					occupiedPositions[x, y] = false;
				}
			}
		}

		//generate 5 keys
		int keyAmount = 0;
		while(keyAmount<5)
        {
			int randomX = Random.Range(1, LevelGenerator.ROOM_WIDTH - 2);
			int randomY = Random.Range(1, LevelGenerator.ROOM_HEIGHT - 2);
			if (!occupiedPositions[randomX, randomY])
			{
				keyAmount++;
				occupiedPositions[randomX, randomY] = true;
				Tile.spawnTile(keyPrefab, transform, randomX, randomY);
			}
			else
				continue;
        }

	}

	protected void roomGenerationVersionFour(LevelGenerator ourGenerator, ExitConstraint requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);
		generateDoor(ourGenerator);

		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
		{
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
			{
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH - 1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1)
				{
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else
				{
					occupiedPositions[x, y] = false;
				}
			}
		}
		// wall and enemy
		int posX = LevelGenerator.ROOM_WIDTH / 2;
		int posY = LevelGenerator.ROOM_HEIGHT / 2;

		for(int i =-2;i<=2;i++)
        {
			for(int j = -2;j<=2;j++)
            {
				int spawnPosX = posX + i;
				int spawnPosY = posY + j;
	
				
				if(IsInRange(spawnPosX,spawnPosY) && !occupiedPositions[spawnPosX,spawnPosY])
                {
					if (i != 0 && j != 0  )
					{
						if(Random.Range(0, 4) == 1)
                        {
							GameObject prefab = Random.Range(0, 2) == 0 ? enemy2Prefab : enemyPrefab;
							Tile.spawnTile(prefab, transform, spawnPosX, spawnPosY);
						}
					}
					else
						Tile.spawnTile(ourGenerator.normalWallPrefab, transform, spawnPosX, spawnPosY);
				}
            }
        }




	}

	protected void roomGenerationVersionFive(LevelGenerator ourGenerator, ExitConstraint requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);
		generateDoor(ourGenerator);

		bool[,] occupiedPositions = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
		{
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
			{
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH - 1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT - 1)
				{
					// All border zones are occupied.
					occupiedPositions[x, y] = true;
				}
				else
				{
					occupiedPositions[x, y] = false;
				}
			}
		}

		// wall and enemy
		int posX = LevelGenerator.ROOM_WIDTH / 2-1;
		int posY = LevelGenerator.ROOM_HEIGHT / 2-1;


		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				
				int spawnPosX = posX + i;
				int spawnPosY = posY + j;
				if (IsInRange(spawnPosX, spawnPosY) && !occupiedPositions[spawnPosX, spawnPosY])
				{
					if (Mathf.Abs(i) != Mathf.Abs(j))
                    {
						if (Random.Range(0, 4) == 1)
						{
							GameObject prefab = Random.Range(0, 2) == 0 ? enemy2Prefab : enemyPrefab;
							Tile.spawnTile(prefab, transform, spawnPosX, spawnPosY);
						}
					}
					else
                    {
						Tile.spawnTile(ourGenerator.normalWallPrefab, transform, spawnPosX, spawnPosY);
					}
						
				}
			}
		}

	}
	bool IsInRange(int x,int y)
    {
		return x >= 0 && x < LevelGenerator.ROOM_WIDTH && y >= 0 && y < LevelGenerator.ROOM_HEIGHT;
    }
	protected void generateDoor(LevelGenerator ourGenerator)
    {
		Tile.spawnTile(doorPrefab, transform, 4, 0);
		Tile.spawnTile(ourGenerator.normalWallPrefab, transform, 5, 0);
		Tile.spawnTile(doorPrefab, transform, 0, 4);
		Tile.spawnTile(doorPrefab, transform, 9, 4);
		Tile.spawnTile(doorPrefab, transform, 4, 7);
		Tile.spawnTile(ourGenerator.normalWallPrefab, transform, 5, 7);
    }

    	protected void generateWalls(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		// Basically we go over the border and determining where to spawn walls.
		bool[,] wallMap = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
					|| y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					
					if (x == LevelGenerator.ROOM_WIDTH/2 
						&& y == LevelGenerator.ROOM_HEIGHT-1
                        && requiredExits.upExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH-1
						     && y == LevelGenerator.ROOM_HEIGHT/2
                             && requiredExits.rightExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH/2
						     && y == 0
                             && requiredExits.downExitRequired) {
						wallMap[x, y] = false;
					}
					else if (x == 0 
						     && y == LevelGenerator.ROOM_HEIGHT/2 
                             && requiredExits.leftExitRequired) {
						wallMap[x, y] = false;
					}
					else {
						wallMap[x, y] = Random.value <= borderWallProbability;
					}
					continue;
				}
				wallMap[x, y] = false;
			}
		}

        wallMap[4, 0] = false;
		wallMap[0, 4] = false;
		wallMap[9, 4] = false;
		wallMap[4, 7] = false;

		// Now actually spawn all the walls.
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (wallMap[x, y]) {
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
				}
			}
		}
	}
}