using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public static RoomCreator Instance;

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
    }

    public static Room Create(Vector2Int scale) {
        Room newRoom = ScriptableObject.CreateInstance(typeof(Room)) as Room;

        newRoom.scale = scale;

        for (int i = 0; i < newRoom.wall.Length; i++) {
            newRoom.wall[i] = Instantiate(Instance.wall);
        }

        newRoom.floor = Instantiate(Instance.floor);
        newRoom.ambientLight = Instantiate(Instance.ambientLight);

        newRoom.doorWall = Instance.doorWall;
        for (int i = 0; i < newRoom.corner.Length; i++) {
            newRoom.corner[i] = Instantiate(Instance.corner);
        }

        return newRoom;
    }

    
}
