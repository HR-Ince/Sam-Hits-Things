using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] bool useTouch = false;
    public bool Pressed { get { return pressed; } }
    public bool PressHeld { get { return pressHeld; } }
    public bool PressReleased { get { return pressReleased; } }
    public Vector3 PressPos { get { return pressPos; } }

    private bool pressed;
    private bool pressHeld;
    private bool pressReleased;
    private Vector3 pressPos;

    private MouseOverUIChecker mouseCheck;

    private void Awake()
    {
        mouseCheck = FindObjectOfType<MouseOverUIChecker>();
    }
    private void Update()
    {
        if (useTouch)
            TouchInput();
        else
            MouseInput();
    }
    private void MouseInput()
    {
        if (mouseCheck.IsOverUI) { return; }

        pressed = Input.GetMouseButtonDown(0);
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
