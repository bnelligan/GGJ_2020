using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    public Camera mainCamera;

    public Transform cursor;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    
    void Start()
    {
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    
    // Update is called once per frame
    void Update()
    {
        // Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        // cursor.position = mousePos;
        
    }
}
