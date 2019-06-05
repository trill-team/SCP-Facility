using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float grabScaling = 1000;
    public float scrollScaling = 10; 

    private Vector3 currentMousePos;
    private Vector3 prevMousePos;

    private float oldTouchFrame;
    private float touchFrame;


    private void Start() {
        this.prevMousePos = Vector2.zero;
        this.currentMousePos = Vector2.zero;
        this.oldTouchFrame = 0;
        this.touchFrame = 0; 
    }
    private void Update() {
#if UNITY_ANDROID
        if (GetInteractive() & Input.touchCount == 1)
#endif
#if UNITY_EDITOR
        if (GetInteractive())
#endif
        {
            if (this.prevMousePos.Equals(Vector2.zero)) {
                this.prevMousePos = GetInteractivePosition();
            }

            this.currentMousePos = GetInteractivePosition();

            Vector3 finalDirection = this.currentMousePos - this.prevMousePos;

            this.gameObject.transform.position += -(new Vector3(finalDirection.x, 0, finalDirection.y)) / grabScaling;

            this.prevMousePos = this.currentMousePos;
        }
        else {
            this.prevMousePos = Vector2.zero;
        }

        this.transform.Translate(this.transform.forward * GetScrollDelta());
    }

    private float GetScrollDelta() {
#if UNITY_EDITOR
        return Input.mouseScrollDelta.y;
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 2) {
            if (oldTouchFrame.Equals(Vector2.zero)) {
                oldTouchFrame = touchFrame;
            }

            this.touchFrame = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

            this.oldTouchFrame = this.touchFrame;

            return (touchFrame - oldTouchFrame);
        }

        return 0;
#endif
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
    private bool GetInteractive() {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#endif
#if UNITY_ANDROID
        return Input.touchCount > 0;
#endif
    }
}
