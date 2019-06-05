using UnityEngine;

public class Room : ScriptableObject {
    public enum WALL_SIDE {
        NONE = -1,

        FORWARD = 2,
        BACKWARD = 0,
        LEFT = 3,
        RIGHT = 1
    };
    public const float WALL_SIZE = .4f;
    public const float WALL_HEIGHT = 1f;

    public const float DEF_X_ROTATION = -90;
    public const float DEF_Y_ROTATION = 0;
    public const float DEF_Z_ROTATION = 0;
    public const float RECT_Y_ROTATION = 90;
    public const float CORNER_SIZE = 1;
    public const float CORNER_AMOUNT = 2;

    public const float CENTER_OF_MODEL = 0.5f;
    public const int FLIP_MODEL = -1;

    public GameObject[] wall = new GameObject[4];
    public GameObject[] corner = new GameObject[4];
    public GameObject doorWall;
    public GameObject floor;
    public GameObject ambientLight;

    public Vector2Int position;
    public Vector2Int scale;

    public static int CornerAffects { get { return (int)(CORNER_SIZE * CORNER_AMOUNT); } private set { } }

    //public List<RoomComponents> components;

    public void Destroy() {
        for (int i = 0; i < this.wall.Length; i++) {
            Destroy(this.wall[i]);
        }
        Destroy(this.floor);
        Destroy(ambientLight);
    }

    public void InitLight() {
    }

    public void InitWalls(WALL_SIDE[] transparentSides, WALL_SIDE[] toDeleteSides)
    {
        Room newRoom = this;
        Vector2Int scale = newRoom.scale;

        Vector2Int flip = Vector2Int.one;

        for (int i = 0; i < newRoom.wall.Length; i++)
        {
            flip = Vector2Int.one;

            GameObject currentWall = newRoom.wall[i];

            if (currentWall == null) continue;

            switch (i)
            {
                case (int)Room.WALL_SIDE.LEFT:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(DEF_X_ROTATION, RECT_Y_ROTATION, DEF_Z_ROTATION);
                    flip = new Vector2Int(1, FLIP_MODEL);
                    break;
                case (int)Room.WALL_SIDE.RIGHT:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(DEF_X_ROTATION, RECT_Y_ROTATION, DEF_Z_ROTATION);
                    break;
                case (int)Room.WALL_SIDE.FORWARD:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(DEF_X_ROTATION, DEF_Y_ROTATION, DEF_Z_ROTATION);
                    flip = new Vector2Int(1, FLIP_MODEL);
                    break;
                default:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(DEF_X_ROTATION, DEF_Y_ROTATION, DEF_Z_ROTATION);
                    break;
            }
            newRoom.wall[i].transform.localScale = new Vector3((scale.y - CornerAffects) * CENTER_OF_MODEL * flip.x, WALL_SIZE * flip.y, WALL_HEIGHT);
            newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.mainMaterial;
            newRoom.corner[i].GetComponent<Renderer>().material = RoomCreator.Instance.mainMaterial;
            newRoom.corner[i].transform.localScale = Vector3.one;

            for (int z = 0; z < transparentSides.Length; z++)
            {
                if (i == (int)transparentSides[z])
                {
                    newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.transparentMaterial;
                    newRoom.corner[i].GetComponent<Renderer>().material = RoomCreator.Instance.transparentMaterial;
                }
            }
            if (toDeleteSides.Length <= 0) continue;
            for (int z = 0; z < toDeleteSides.Length; z++)
            {
                if (i == (int)toDeleteSides[z])
                {
                    newRoom.wall[i].SetActive(false);
                    newRoom.corner[i].SetActive(false);
                }
            }
        }
        newRoom.floor.GetComponent<Renderer>().material = RoomCreator.Instance.floorMaterial;
        newRoom.floor.transform.localScale = new Vector3(scale.x * CENTER_OF_MODEL, scale.y * CENTER_OF_MODEL, 0.1f);
    }
    public void InitWalls(WALL_SIDE[] transparentSides)
    {
        this.InitWalls(transparentSides, new WALL_SIDE[] { });
    }
    public void InitWalls() {
        this.InitWalls(new WALL_SIDE[] { WALL_SIDE.FORWARD, WALL_SIDE.RIGHT });
    }

    public void InitWalls(WALL_SIDE toHide, WALL_SIDE toDestroy) {
        this.InitWalls(new WALL_SIDE[] { toHide }, new WALL_SIDE[] { toDestroy });
    }

    public void AddDoor() {}
    public void RemoveDoor() {
    }

    public void UpdateWalls(WALL_SIDE[] transparentSides, WALL_SIDE[] toDeleteSides, WALL_SIDE[] toShow, WALL_SIDE[] toVisible)
    {
        Room newRoom = this;
        Vector2Int scale = newRoom.scale;

        for (int i = 0; i < newRoom.wall.Length; i++)
        {
            if (newRoom.wall[i] == null) continue;

            for (int z = 0; z < transparentSides.Length; z++)
            {
                if (i == (int)transparentSides[z])
                {
                    newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.transparentMaterial;
                }
                if (toVisible.Length <= 0) continue;
                if (i == (int)toVisible[z < toVisible.Length ? z : 0])
                {
                    newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.mainMaterial;
                }
                if (toDeleteSides.Length <= 0) continue;
                if (i == (int)toDeleteSides[z < toDeleteSides.Length ? z : 0])
                {
                    newRoom.wall[i].SetActive(false);
                }
                if (toShow.Length <= 0) continue;
                if (i == (int)toShow[z < toShow.Length ? z : 0])
                {
                    newRoom.wall[i].SetActive(true);
                }
            }
        }
    }
    public void UpdateWalls(WALL_SIDE[] transparentSides, WALL_SIDE[] toDeleteSides) {
        this.UpdateWalls(transparentSides, toDeleteSides, new Room.WALL_SIDE[] { Room.WALL_SIDE.NONE}, new Room.WALL_SIDE[] { Room.WALL_SIDE.NONE });
    }
    public void UpdateWalls(WALL_SIDE transparentSide, WALL_SIDE toDeleteSide) {
        this.UpdateWalls(
            new WALL_SIDE[] { transparentSide },
            new WALL_SIDE[] { toDeleteSide }
        );
    }
}
