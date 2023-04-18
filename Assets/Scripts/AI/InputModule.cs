using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputModule : StandaloneInputModule
{
    public void ClickAt(float x, float y)
    {
        Input.simulateMouseWithTouches = true;
        var pointerData = GetTouchPointerEventData(new Touch()
        {
            position = new Vector2(x, y),
        }, out bool b, out bool bb);

        ProcessTouchPress(pointerData, true, true);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F1))
        // {
        //     Debug.Log(Input.mousePosition);
        //     ClickAt(Screen.width / 2, Screen.height / 2);
        // }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(Input.mousePosition);
            ClickAt(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}