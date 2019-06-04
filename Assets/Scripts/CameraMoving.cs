using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float scaling = 1000;

    private Vector3 currentMousePos;
    private Vector3 prevMousePos;


    private void Start() {
        this.prevMousePos = Vector2.zero;
        this.currentMousePos = Vector2.zero;
    }
    private void Update() {
        if (Input.GetMouseButton(0))
        {
            if (this.prevMousePos.Equals(Vector2.zero)) {
                this.prevMousePos = GetInteractivePosition();
            }

            this.currentMousePos = GetInteractivePosition();

            Vector3 finalDirection = this.currentMousePos - this.prevMousePos;

            this.gameObject.transform.position += -(new Vector3(finalDirection.x, 0, finalDirection.y)) / scaling;

            this.prevMousePos = this.currentMousePos;
        }
        else {
            this.prevMousePos = Vector2.zero;
        }
    }

    private Vector3 GetInteractivePosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#endif
#if UNITY_ANDROID
        return Input.GetTouch(0).position;
#endif
    }
}
