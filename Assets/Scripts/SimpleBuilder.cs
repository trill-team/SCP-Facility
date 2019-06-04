using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBuilder : MonoBehaviour
{
    private const int INCREMENT = 10;
    private int x = 0;
    private int y = 0;

    private GameObject cursor;

    private void Start() {
        this.cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.cursor.transform.localScale = new Vector3(10, 1, 10);
        this.cursor.GetComponent<Renderer>().material.color = Color.green;
    }
    private void FixedUpdate() {
        updateCursor();
    }

    public void BuildSomething()
    {
        RoomManager.Place(
            RoomCreator.Create(new Vector2Int(10, 10)),
            new Vector2Int(INCREMENT * x, INCREMENT * y)
        );
    }

    public void MoveCursorX(int delta) {
        this.x += delta;
    }
    public void MoveCursorY(int delta) {
        this.y += delta;
    }
    
    private void updateCursor() {
        this.cursor.transform.position = new Vector3(x * INCREMENT + 5, 0, y * INCREMENT + 5);
    }
}
