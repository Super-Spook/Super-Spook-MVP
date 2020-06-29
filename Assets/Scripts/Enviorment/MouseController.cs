using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = false;
    }
    void FixedUpdate()
    {
        MoveSpook();
    }

    private void MoveSpook()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
    }
}
