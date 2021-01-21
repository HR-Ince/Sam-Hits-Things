using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameEvent OnPress;
    [SerializeField] GameEvent OnRelease;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (OnPress != null)
                OnPress.Invoke();

        if (Input.GetMouseButtonUp(0))
            if (OnRelease != null)
                OnRelease.Invoke();
    }
}
