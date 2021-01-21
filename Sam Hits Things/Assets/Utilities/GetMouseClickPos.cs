using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMouseClickPos : MonoBehaviour
{
    [SerializeField] MouseData mouseClickPos;

    public void StoreMousePos()
    {
        Vector2 mousePosOnClick = Input.mousePosition;
        float posAsHeightPercentage = (mousePosOnClick.y / Screen.height) * 100;
        float posAsWidthPercentage = (mousePosOnClick.x / Screen.width) * 100;

        mouseClickPos.ClickPos = new Vector2(posAsWidthPercentage, posAsHeightPercentage);
        print("Position: " + mouseClickPos.ClickPos);
    }
    public void StoreMouseDragLength()
    {
        Vector2 mousePos = Input.mousePosition;
        float xPosAsPercentageofScreen = (mousePos.x / Screen.width) * 100;
        float distance = Mathf.Abs(mouseClickPos.ClickPos.x - xPosAsPercentageofScreen);

        if (distance > 10f)
            mouseClickPos.DragLength = distance;
        else
            mouseClickPos.DragLength = 0f;

        print("Distance: " + mouseClickPos.DragLength); 
    }
}
