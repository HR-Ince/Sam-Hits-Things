using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLaunchClickData : MonoBehaviour
{
    [SerializeField] MouseData mouseData;
    [SerializeField] PlayerRegister playerRegister;

    private Vector3 pressPos;
    private int dragLength;

    public void StoreMousePos()
    {
        Vector3 player = Camera.main.WorldToScreenPoint(playerRegister.PlayerOne.transform.position);
        pressPos = Input.mousePosition;
        float distance = Vector3.Distance(player, pressPos);

        Vector3 directionalVector = (pressPos - player) / distance;
        mouseData.ClickPos = directionalVector;
        print("Direction: " + directionalVector);
    }
    public void GetDragLength()
    {
        Vector2 mousePos = Input.mousePosition;
        float xPosAsPercentageofScreen = (mousePos.x / Screen.width) * 100;
        float pressPosXAsPercentageofScreen = (pressPos.x / Screen.width) * 100;
        float dragLengthf = Mathf.Clamp(Mathf.Abs(xPosAsPercentageofScreen - pressPosXAsPercentageofScreen) * 2, 0, 100);
        dragLength = (int)dragLengthf;

    }
    public void StoreDragLength()
    {
        
        if (dragLength > 5f)
            mouseData.DragLength = dragLength;
        else
            mouseData.DragLength = 0f;

        print("Drag Length: " + dragLength);
    }

    public void ResetDragLength()
    {
        mouseData.DragLength = 0;
    }
}
