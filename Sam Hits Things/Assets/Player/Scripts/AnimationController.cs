using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator anim;
    PlayerInput input;
    Movement launcher;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if(anim == null) { Debug.LogError("Animator missing from " + transform.parent.gameObject.name); }
        input = GetComponentInParent<PlayerInput>();
        if (input == null) { Debug.LogError("Player Input missing from " + transform.parent.gameObject.name); }
        launcher = GetComponentInParent<Movement>();
        if(launcher == null) { Debug.LogError("Movement missing from " + transform.parent.gameObject.name); }
    }

    private void Update()
    {
        if (input.PressHeld)
        {
            SetCrouching(true);
        }
    }

    public void SetCrouching(bool value)
    {
        anim.SetBool("isCrouching", value);
    }
}
