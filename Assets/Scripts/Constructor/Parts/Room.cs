using UnityEngine;

public class Room : ScriptableObject {
    public enum WALL_SIDE {
        NONE = -1,

        FORWARD = 2,
        BACKWARD = 0,
        LEFT = 3,
        RIGHT = 1
    };
    public const float WALL_SIZE = .2f;

    public GameObject[] wall = new GameObject[4];
    public GameObject floor;

    public Vector2Int position;
    public Vector2Int scale;

    public void Destroy() {
        for (int i = 0; i < this.wall.Length; i++) {
            Destroy(this.wall[i]);
        }
        Destroy(this.floor);
    }

    public void InitWalls(WALL_SIDE[] transparentSides, WALL_SIDE[] toDeleteSides)
    {
        Room newRoom = this;
        Vector2Int scale = newRoom.scale;

        for (int i = 0; i < newRoom.wall.Length; i++)
        {
            if (newRoom.wall[i] == null) continue;

            switch (i)
            {
                case (int)Room.WALL_SIDE.LEFT:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case (int)Room.WALL_SIDE.RIGHT:
                    newRoom.wall[i].transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
            }
            newRoom.wall[i].transform.localScale = new Vector3(scale.y, 1, Room.WALL_SIZE);
            newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.mainMaterial;

            for (int z = 0; z < transparentSides.Length; z++)
            {
                if (i == (int)transparentSides[z])
                {
                    newRoom.wall[i].GetComponent<Renderer>().material = RoomCreator.Instance.transparentMaterial;
                }
            }
            if (toDeleteSides.Length <= 0) continue;
            for (int z = 0; z < toDeleteSides.Length; z++)
            {
                if (i == (int)toDeleteSides[z])
                {
                    newRoom.wall[i].SetActive(false);
                }
            }
        }
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
