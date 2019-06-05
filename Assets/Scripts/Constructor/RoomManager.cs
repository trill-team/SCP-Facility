﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Vector2Int fieldSize;

    public Room[,] cells;

    public List<Room> allRooms = new List<Room>();

    private void Awake() {
        Instance = this;
        cells = new Room[fieldSize.x, fieldSize.y];
    }

    public static void Place(Room roomToPlace, Vector2Int position) {
        if (Instance.cells[position.x + 1, position.y + 1] != null || Instance.cells[position.x + roomToPlace.scale.x - 1, position.y + roomToPlace.scale.y - 1] != null
            || Instance.cells[position.x + roomToPlace.scale.x - 1, position.y] != null || Instance.cells[position.x, position.y + roomToPlace.scale.y - 1] != null) {
            roomToPlace.Destroy();
            return;
        }
        bool topRightCorner = CheckBorderCollision(position + roomToPlace.scale, new Vector2Int(2, 2));
        bool topLeftCorner = CheckBorderCollision(position + new Vector2Int(0, roomToPlace.scale.y), new Vector2Int(-2, 2));
        bool bottomRightCorner = CheckBorderCollision(position + new Vector2Int(roomToPlace.scale.x, 0), new Vector2Int(2, -2));
        bool bottomLeftCorner = CheckBorderCollision(position, new Vector2Int(-2, -2));

        bool top = CheckBorderCollision(position + roomToPlace.scale, new Vector2Int(-1, 1));
        bool bottom = CheckBorderCollision(position, new Vector2Int(0, -1));
        bool left = CheckBorderCollision(position, new Vector2Int(-1, 0));
        bool right = CheckBorderCollision(position + roomToPlace.scale, new Vector2Int(1, -1));

        if ((left & bottomLeftCorner & bottom) || (right & topRightCorner & top) || (left & topLeftCorner & top) || (right & bottomRightCorner & bottom))
        {
            roomToPlace.Destroy();
            return;
        }        
        

        for (int i = 0; i < roomToPlace.scale.x; i++) {
            for (int j = 0; j < roomToPlace.scale.y; j++) {
                Instance.cells[position.x + i, position.y + j] = roomToPlace;
            }
        }
        // check border collisions
        List<Room.WALL_SIDE> toHide = new List<Room.WALL_SIDE>();
        List<Room.WALL_SIDE> toDestroy = new List<Room.WALL_SIDE>();

        toHide.AddRange(new Room.WALL_SIDE[] { Room.WALL_SIDE.FORWARD, Room.WALL_SIDE.RIGHT });
        toDestroy.Add(Room.WALL_SIDE.NONE);

        if (right) {            
            Instance.cells[position.x + roomToPlace.scale.x + 1, position.y + roomToPlace.scale.y - 1].
                UpdateWalls(Room.WALL_SIDE.NONE, Room.WALL_SIDE.LEFT);
        }
        if (top)
        {
            toDestroy.Add(Room.WALL_SIDE.BACKWARD);
        }
        if (left) {
            toDestroy.Add(Room.WALL_SIDE.LEFT);
        }
        if (bottom)
        {
            toHide.Add(Room.WALL_SIDE.FORWARD);
            Instance.cells[position.x, position.y - 1].UpdateWalls(Room.WALL_SIDE.BACKWARD, Room.WALL_SIDE.BACKWARD);
        }

        // init walls
        roomToPlace.InitLight();
        roomToPlace.InitWalls(toHide.ToArray(), toDestroy.ToArray());

        // place all room
        PlaceRoom(roomToPlace, position, roomToPlace.scale);

        Instance.allRooms.Add(roomToPlace);
    }
    private static void PlaceRoom(Room roomToPlace, Vector2Int position, Vector2 scale) {
        PlaceObject(roomToPlace.wall[(int)Room.WALL_SIDE.FORWARD], position + new Vector2Int((roomToPlace.scale.x ) / 2, 0));
        PlaceObject(roomToPlace.wall[(int)Room.WALL_SIDE.LEFT], position + new Vector2Int(0, (roomToPlace.scale.y ) / 2));
        PlaceObject(roomToPlace.wall[(int)Room.WALL_SIDE.BACKWARD], position + new Vector2Int((roomToPlace.scale.x) / 2, roomToPlace.scale.y));
        PlaceObject(roomToPlace.wall[(int)Room.WALL_SIDE.RIGHT], position + new Vector2Int(roomToPlace.scale.x, (roomToPlace.scale.y) / 2));

        PlaceObject(roomToPlace.corner[(int)Room.WALL_SIDE.BACKWARD], position + new Vector2Int(0, roomToPlace.scale.y));
        PlaceObject(roomToPlace.corner[(int)Room.WALL_SIDE.LEFT], position + new Vector2Int(roomToPlace.scale.x, roomToPlace.scale.y));
        PlaceObject(roomToPlace.corner[(int)Room.WALL_SIDE.FORWARD], position + new Vector2Int(roomToPlace.scale.x, 0));
        PlaceObject(roomToPlace.corner[(int)Room.WALL_SIDE.RIGHT], position + new Vector2Int(0, 0));

        for (int i = 0; i < roomToPlace.wall.Length; i++) {
            roomToPlace.wall[i].transform.position += Vector3.up * Room.WALL_HEIGHT / 2;
        }

        PlaceObject(roomToPlace.floor, position + new Vector2Int(roomToPlace.scale.x, roomToPlace.scale.y));
        PlaceObject(roomToPlace.ambientLight, position + new Vector2Int(roomToPlace.scale.x / 2, roomToPlace.scale.y / 2));
    }
    private static void PlaceObject(GameObject toPlace, Vector2Int position) {
        if (toPlace != null)
            toPlace.transform.position = new Vector3(position.x, 0, position.y);
    }
    private static bool CheckBorderCollision(Vector2Int position, Vector2Int cursor) {
        int x = position.x + cursor.x;
        int y = position.y + cursor.y;

        if (x > Instance.cells.GetLength(0) || y > Instance.cells.GetLength(1) || x < 0 || y < 0) {
            return false;
        }

        if (Instance.cells[x, y] != null) {
            return true;
        }
        return false;
    }
}
