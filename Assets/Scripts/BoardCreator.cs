using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange (3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange (3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange (6, 10);    // The range of lengths corridors between rooms can have.
    public RuleTile[] floorTiles;                           // An array of floor tile prefabs.
    public RuleTile[] wallTiles;                            // An array of wall tile prefabs.
    public RuleTile[] outerWallTiles;    
    public EnemyController[] enemies;                   // An array of outer wall tile prefabs.
    public GameObject player;
	public Tilemap backgroundmap, wallmap;
    public NavMeshObstacle obstacle;
    public NavMeshSurface surface;
    
    private GameObject PlayerObject;
    private List<EnemyController> InstantiatedEnemies;
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.


    private void Start ()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");
        InstantiatedEnemies = new List<EnemyController>();

        SetupTilesArray ();

        CreateRoomsAndCorridors ();

        SetTilesValuesForRooms ();
        SetTilesValuesForCorridors ();

        InstantiateTiles ();
        //InstantiateOuterWalls ();
        surface.BuildNavMesh();
        for(int i = 0; i < InstantiatedEnemies.Count; ++i) { 
            InstantiatedEnemies[i].player = PlayerObject;
        }

    }


    void SetupTilesArray ()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];
        
        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors ()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room ();
        corridors[0] = new Corridor ();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room ();
            
            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor ();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
            if (i == (int) (rooms.Length *.5f))
            {
                Vector3 playerPos = new Vector3 (rooms[i].xPos + rooms[i].roomWidth/2, rooms[i].yPos + rooms[i].roomHeight/2, 0);
                PlayerObject = Instantiate(player, playerPos, Quaternion.identity);
            }

            int randomEnemySpawn = Random.Range(0, 10);
            if(randomEnemySpawn < 2) {

                int randomIndex = Random.Range(0, enemies.Length);
                Vector3 playerPos = new Vector3 (rooms[i].xPos + rooms[i].roomWidth/2, rooms[i].yPos + rooms[i].roomHeight/2, 0);
                InstantiatedEnemies.Add(Instantiate(enemies[randomIndex], playerPos, Quaternion.identity));
            }
        }

    }


    void SetTilesValuesForRooms ()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            
            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors ()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles ()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // ... and instantiate a floor tile for it.

                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    if(i==0 || i == tiles.Length-1 || j == 0 || j == tiles.Length-1 || tiles[i+1][j] != TileType.Wall
                    || tiles[i-1][j] != TileType.Wall
                    || tiles[i][j+1] != TileType.Wall
                    || tiles[i][j-1] != TileType.Wall){
                    // ... instantiate a wall over the top.
                        InstantiateFromArray (true, wallTiles, i, j);

                    }
                }
                else {
                    InstantiateFromArray (false, floorTiles, i, j);

                }
            }
        }
    }


    void InstantiateOuterWalls ()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(true, outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray (true, outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    void InstantiateFromArray (bool wall, RuleTile[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3Int position = new Vector3Int((int) xCoord, (int) yCoord, 0);

        // Create an instance of the prefab from the random index of the array.
        backgroundmap.SetTile(position, prefabs[randomIndex]);
        if(wall) { 
            wallmap.SetTile(position, prefabs[randomIndex]);
            Instantiate(obstacle, wallmap.GetCellCenterWorld(position), Quaternion.identity);
        }
    }
}