using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool Pressed { get { return pressed; } }
    public bool PressHeld { get { return pressHeld; } }
    public bool PressReleased { get { return pressReleased; } }
    public Vector3 PressPos { get { return pressPos; } }

    private bool pressed;
    private bool pressHeld;
    private bool pressReleased;
    private Vector3 pressPos;

    private Camera cam;
    private MouseOverUIChecker mouseCheck;

    private void Awake()
    {
        cam = Camera.main;
        mouseCheck = FindObjectOfType<MouseOverUIChecker>();
    }
    private void Update()
    {
        if (mouseCheck.IsOverUI) { return; }

        pressed = Input.GetMouseButtonDown(0);
        pressHeld = Input.GetMouseButton(0);
        pressReleased = Input.GetMouseButtonUp(0);
        pressPos = Input.mousePosition;
    }
}
