using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBuilder : MonoBehaviour
{
    public string componentName = "ceilingLamp";

    public GameObject licht;
    public float rayLength = 1000;
    public float lightHeight = 2;

    private const int INCREMENT = 10;
    private int x = 0;
    private int y = 0;

    private GameObject cursor;

    private void Start() {
        this.cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.cursor.transform.localScale = new Vector3(10, 1, 10);
        this.cursor.GetComponent<Renderer>().material.color = Color.green;
    }
    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.UpArrow)) { y += INCREMENT ; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { y -= INCREMENT; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { x += INCREMENT; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { x -= INCREMENT; }

        if (Input.GetKeyDown(KeyCode.Space)) { BuildSomething(); }

        if (Input.GetKeyDown(KeyCode.Y)) {
            Room getRoom = RoomManager.GetRoomAt(new Vector2Int(x, y));
            if (getRoom != null)
                getRoom.AddComponent(RoomComponentManager.Find(this.componentName));
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        this.licht.transform.position = ray.GetPoint(rayLength);
#endif
    }
    private void FixedUpdate() {
        updateCursor();
    }

    public void BuildSomething()
    {
        RoomManager.Place(
            RoomCreator.Create(new Vector2Int(10, 10)),
            new Vector2Int(x, y)
        );
    }

    public void MoveCursorX(int delta) {
        this.x += delta;
    }
    public void MoveCursorY(int delta) {
        this.y += delta;
    }
    
    private void updateCursor() {
        this.cursor.transform.position = RoomManager.GetLocation(new Vector2Int(x + 5, y + 5));
    }
}
