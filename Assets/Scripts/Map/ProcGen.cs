using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// never to be inherited
sealed class ProcGen
{
    // Generate new dungeon map
    public void GenerateDungeon(
        int mapWidth,
        int mapHeight,
        int roomMinSize,
        int roomMaxSize,
        int maxRooms,
        int maxMonstersPerRoom,
        int maxItemsPerRoom,
        List<RectangularRoom> rooms,
        bool isNewGame)
    {
        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(roomMinSize, roomMaxSize);
            int roomHeight = Random.Range(roomMinSize, roomMaxSize);

            int roomX = Random.Range(0, mapWidth - roomWidth - 1);
            int roomY = Random.Range(0, mapHeight - roomHeight - 1);

            RectangularRoom newRoom = new(
                roomX,
                roomY,
                roomWidth,
                roomHeight);

            // check if rooms are overlaping - if yes, skip generating the room
            if (newRoom.Overlaps(rooms))
            {
                continue;
            }

            // build walls around the rooms
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX ||
                        x == roomX + roomWidth - 1 ||
                        y == roomY ||
                        y == roomY + roomHeight - 1)
                    {
                        if (SetWallTileIfEmpty(new Vector3Int(x, y)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y));
                    }
                }
            }

            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], newRoom);
            }

            PlaceEntities(newRoom, maxMonstersPerRoom, maxItemsPerRoom);

            rooms.Add(newRoom);
        }

        // add stairs to the last room
        MapManager.instance.FloorMap.SetTile((Vector3Int)rooms[rooms.Count - 1].RandomPoint(), MapManager.instance.DownStairsTile);

        // add Player to the first room
        Vector3Int playerPos = (Vector3Int)rooms[0].RandomPoint();

        while (GameManager.instance.GetActorAtLocation(playerPos) is not null)
        {
            playerPos = (Vector3Int)rooms[0].RandomPoint();
        }

        MapManager.instance.FloorMap.SetTile(playerPos, MapManager.instance.UpStairsTile);

        if (!isNewGame)
        {
            GameManager.instance.Actors[0].transform.position = new Vector3(playerPos.x + 0.5f, playerPos.y + 0.5f, 0);
        }
        else
        {
            MapManager.instance.CreateEntity("Player", (Vector2Int)playerPos);
        }
    }

    // Return an L-shaped tunnel between rooms using Bresenham lines
    private void TunnelBetween(RectangularRoom oldRoom, RectangularRoom newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            // Move horizontally, then vertically
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            // Move vertically, then horizontally
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        //Generate the coordinates for the tunnel
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // set tiles for the tunnel
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (SetWallTileIfEmpty(new Vector3Int(x, y)))
                    {
                        continue;
                    }
                }
            }
        }
    }

    private bool SetWallTileIfEmpty(Vector3Int pos)
    {
        if (MapManager.instance.FloorMap.GetTile(pos))
        {
            return true;
        }
        else
        {
            MapManager.instance.ObstacleMap.SetTile(pos,
                MapManager.instance.WallTile);
            return false;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        if (MapManager.instance.ObstacleMap.GetTile(pos))
        {
            MapManager.instance.ObstacleMap.SetTile(pos, null);
        }
        MapManager.instance.FloorMap.SetTile(pos,
            MapManager.instance.FloorTile);
    }


    private void PlaceEntities(RectangularRoom newRoom, int maximumMonsters, int maximumItems)
    {
        int numberOfMonsters = Random.Range(0, maximumMonsters + 1);
        int numberOfItems = Random.Range(0, maximumItems + 1);

        for (int monster = 0; monster < numberOfMonsters;)
        {
            int x = Random.Range(newRoom.X, newRoom.X + newRoom.Width);
            int y = Random.Range(newRoom.Y, newRoom.Y + newRoom.Height);

            if (x == newRoom.X || x == newRoom.X + newRoom.Width - 1 || y == newRoom.Y || y == newRoom.Y + newRoom.Height - 1)
            {
                continue;
            }

            for (int actor = 0; actor < GameManager.instance.Actors.Count; actor++)
            {
                Vector3Int pos = MapManager.instance.FloorMap.WorldToCell(GameManager.instance.Actors[actor].transform.position);

                if (pos.x == x && pos.y == y)
                {
                    return;
                }
            }

            if (Random.value < 0.8f)
            {
                MapManager.instance.CreateEntity("Commoner", new Vector2(x, y));
            }
            else
            {
                MapManager.instance.CreateEntity("Templar", new Vector2(x, y));
            }
            monster++;
        }

        for (int item = 0; item < numberOfItems;)
        {
            int x = Random.Range(newRoom.X, newRoom.X + newRoom.Width);
            int y = Random.Range(newRoom.Y, newRoom.Y + newRoom.Height);

            if (x == newRoom.X || x == newRoom.X + newRoom.Width - 1 || y == newRoom.Y || y == newRoom.Y + newRoom.Height - 1)
            {
                continue;
            }

            for (int entity = 0; entity < GameManager.instance.Entities.Count; entity++)
            {
                Vector3Int pos = MapManager.instance.FloorMap.WorldToCell(GameManager.instance.Entities[entity].transform.position);

                if (pos.x == x && pos.y == y)
                {
                    return;
                }
            }

            float randomValue = Random.value;

            if (randomValue < 0.7f)
            {
                MapManager.instance.CreateEntity("Neon Blood Vial", new Vector2(x, y));
            }
            else if (randomValue < 0.8f)
            {
                MapManager.instance.CreateEntity("Neon Ball Chip", new Vector2(x, y));
            }
            else if (randomValue < 0.9f)
            {
                MapManager.instance.CreateEntity("Confusion Chip", new Vector2(x, y));
            }
            else
            {
                MapManager.instance.CreateEntity("Neon Bolt Chip", new Vector2(x, y));
            }

            item++;
        }
    }
}
