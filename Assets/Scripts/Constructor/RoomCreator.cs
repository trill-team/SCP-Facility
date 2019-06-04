using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public static RoomCreator Instance;

    public Material mainMaterial;
    public Material transparentMaterial;
    public Material floorMaterial;

    private void Awake() {
        Instance = this;
    }

    public static Room Create(Vector2Int scale) {
        Room newRoom = ScriptableObject.CreateInstance(typeof(Room)) as Room;

        newRoom.scale = scale;

        for (int i = 0; i < newRoom.wall.Length; i++) {
            newRoom.wall[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        newRoom.floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newRoom.floor.GetComponent<Renderer>().material = Instance.floorMaterial;
        newRoom.floor.transform.localScale = new Vector3(scale.x, 0.1f, scale.y);

        return newRoom;
    }

    
}
