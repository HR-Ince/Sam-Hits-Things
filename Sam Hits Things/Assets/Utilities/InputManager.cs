using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] UnityEvent onPress;
    [SerializeField] UnityEvent onPressHeld;
    [SerializeField] UnityEvent onRelease;
    [SerializeField] GameEvent gameEventOnRelease;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (onPress != null)
                onPress.Invoke();

        if (Input.GetMouseButton(0))
            if (onPressHeld != null)
                onPressHeld.Invoke();

        if (Input.GetMouseButtonUp(0))
        {
            if (onRelease != null)
            { onRelease.Invoke(); }
            if (gameEventOnRelease != null)
            { gameEventOnRelease.Invoke(); }
        }
            
    }
}
