using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] UnityEvent onPress;
    [SerializeField] GameEvent onPressGE;
    [SerializeField] UnityEvent onPressHeld;
    [SerializeField] GameEvent onPressHeldGE;
    [SerializeField] UnityEvent onRelease;
    [SerializeField] GameEvent onReleaseGE;
    [SerializeField] UnityEvent afterRelease;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (onPress != null)
                onPress.Invoke();
            if (onPressGE != null)
                onPressGE.Invoke();
        }
        if (Input.GetMouseButton(0))
        {
            if (onPressHeld != null)
                onPressHeld.Invoke();
            if (onPressHeldGE != null)
                onPressHeldGE.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (onRelease != null)
            { onRelease.Invoke(); }
            if (onReleaseGE != null)
            { onReleaseGE.Invoke(); }
            if (afterRelease != null)
                afterRelease.Invoke();
        }   
    }
}
