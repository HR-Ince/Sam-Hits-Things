using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool useTouch = false;
    [SerializeField] MouseOverUIChecker uiCheck;

    public bool WasUIClicked { set { wasUIClicked = value; } }
    public bool Pressed { get { return pressed; } }
    public bool PressHeld { get { return pressHeld; } }
    public bool PressReleased { get { return pressReleased; } }
    public Vector3 PressPos { get { return pressPos; } }

    private bool pressed;
    private bool pressHeld;
    private bool pressReleased;
    private Vector3 pressPos;

    private bool wasUIClicked;

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
        if (uiCheck.IsOverUI) { return; }

        pressed = Input.GetMouseButtonDown(0) && !wasUIClicked;
        pressHeld = Input.GetMouseButton(0);
        pressReleased = Input.GetMouseButtonUp(0);
        pressPos = Input.mousePosition;
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            pressed = touch.phase == TouchPhase.Began;
            pressHeld = touch.phase == TouchPhase.Moved;
            pressReleased = touch.phase == TouchPhase.Ended;
            pressPos = touch.position;
        }
            
    }
}
