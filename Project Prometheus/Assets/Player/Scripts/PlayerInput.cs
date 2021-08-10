using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool useTouch = false;
    [SerializeField] MouseOverUIChecker uiCheck;

    public int TouchID { get { return touchID; } }
    public bool GamePressed { get { return gamePressed; } }
    public bool UIPressed { get { return uiPressed; } }
    public bool PressHeld { get { return pressHeld; } }
    public bool PressReleased { get { return pressReleased; } }
    public Vector3 PressPos { get { return pressPos; } }

    private int touchID;
    private bool gamePressed;
    private bool uiPressed;
    private bool uiPress;
    private bool pressHeld;
    private bool pressReleased;
    private Vector3 pressPos;

    private delegate void Controls();

    private Controls input;


    private void Awake()
    {
        if (useTouch) { input = TouchInput; }
        else { input = MouseInput; }

    }
    private void Update()
    {
        input();
    }
    private void MouseInput()
    {
        pressPos = Input.mousePosition;

        if (uiCheck.GetIsOverUI(pressPos)) { uiPressed = Input.GetMouseButtonDown(0); }
        else { gamePressed = Input.GetMouseButtonDown(0); }
                
        pressHeld = Input.GetMouseButton(0);
        pressReleased = Input.GetMouseButtonUp(0);
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchID = touch.fingerId;
            pressPos = touch.position;

            if (uiCheck.GetIsOverUI(pressPos))
            {
                uiPressed = touch.phase == TouchPhase.Began;
            }
            else
            {
                gamePressed = touch.phase == TouchPhase.Began;
            }

            pressHeld = touch.phase == TouchPhase.Moved;
            pressReleased = touch.phase == TouchPhase.Ended;
        }   
    }
}
