using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool pressInput;
    private bool pressHeldInput;
    private bool pressReleasedInput;

    public bool PressInput { get { return pressInput; } }
    public bool PressHeldInput { get { return pressHeldInput; } }
    public bool PressReleasedInput { get { return pressReleasedInput; } }

    private void Update()
    {
        pressInput = Input.GetMouseButtonDown(0);
        pressHeldInput = Input.GetMouseButton(0);
        pressReleasedInput = Input.GetMouseButtonUp(0);
    }
}
