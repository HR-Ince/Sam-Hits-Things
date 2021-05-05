using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool pressed;
    private bool pressHeld;
    private bool pressReleased;
    private Vector3 pressPos;

    public bool Pressed { get { return pressed; } }
    public bool PressHeld { get { return pressHeld; } }
    public bool PressReleased { get { return pressReleased; } }
    public Vector3 PressPos { get { return pressPos; } }

    private void Update()
    {
        pressed = Input.GetMouseButtonDown(0);
        pressHeld = Input.GetMouseButton(0);
        pressReleased = Input.GetMouseButtonUp(0);
        pressPos = Input.mousePosition;
    }
}
