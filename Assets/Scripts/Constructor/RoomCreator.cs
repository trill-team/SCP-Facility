using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public static RoomCreator Instance;

    public List<RoomConfig> roomConfigurations = new List<RoomConfig>();

    public GameObject wall;
    public GameObject corner;
    public GameObject floor;
    public GameObject doorWall;

    public GameObject ambientLight;

    public Material mainMaterial;
    public Material transparentMaterial;
    public Material floorMaterial;
    
    public const int BLENDER_CONVERTATION = 100;

    private void Awake() {
        Instance = this;

        for (int i = 0; i < this.roomConfigurations.Count; i++) {
            for (int j = 0; j < this.roomConfigurations[i].walls.Length; j++) {
                if (this.roomConfigurations[i].walls[j][0] == null) {
                    this.roomConfigurations[i].walls[j][0] = this.wall;
                }
            }
        }
    }

    public static Room Create(Room.TYPE typeOfRoom) {
        List<RoomConfig> workingRooms = GetRoomsOfType(typeOfRoom);
        Room newRoom = ConfigToRoom(workingRooms[Random.Range(0, workingRooms.Count - 1)]);

        return Create(newRoom, newRoom.scale);
    } 
    public static Room Create(Room roomToCreate, Vector2Int scale) {
        Room newRoom = roomToCreate;

        newRoom.scale = scale;

        for (int i = 0; i < newRoom.wall.Length; i++) {
            for (int j = 0; j < newRoom.wall[i].Length; j++) {
                newRoom.wall[i][j] = Instantiate(Instance.wall);
            }
        }

        newRoom.floor = Instantiate(Instance.floor);
        newRoom.ambientLight = Instantiate(Instance.ambientLight);

        newRoom.doorWall = Instance.doorWall;
        for (int i = 0; i < newRoom.corner.Length; i++) {
            newRoom.corner[i] = Instantiate(Instance.corner);
        }

        return newRoom;
    }

    private static Room ConfigToRoom(RoomConfig roomConfig) {
        Room newRoom = ScriptableObject.CreateInstance<Room>();

        newRoom.scale = roomConfig.scale;
        newRoom.wall = roomConfig.walls;
        newRoom.type = roomConfig.roomType;

        return newRoom;
    }
    private static List<RoomConfig> GetRoomsOfType(Room.TYPE type) {
        return Instance.roomConfigurations.FindAll(x => x.roomType == type);
    }
}
