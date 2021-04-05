using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private bool wasAcceptedPress;

    private Animator anim;
    private PlayerInput input;
    private Rigidbody rB;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if(anim == null) { Debug.LogError("Animator missing from " + transform.parent.gameObject.name); }
        input = GetComponentInParent<PlayerInput>();
        if (input == null) { Debug.LogError("Player Input missing from " + transform.parent.gameObject.name); }
        rB = GetComponent<Rigidbody>();
        if(rB == null) { Debug.LogError("Rigidbody missing from " + transform.parent.name); }
    }

    private void Update()
    {
        if (input.Pressed && rB.velocity == Vector3.zero)
            wasAcceptedPress = true;
        if (input.PressHeld && wasAcceptedPress)
            SetCrouching(true);
        if (input.PressReleased)
        {
            SetCrouching(false);
            wasAcceptedPress = false;
        }
            

        if(rB.velocity.y < 0)
        {
            SetJumping(false);
            SetFalling(true);
        }            

        if (anim.GetBool("isFalling") && rB.velocity.y == 0)
            SetFalling(false);

    }
    public void ManageJump()
    {
        SetCrouching(false);
        SetJumping(true);
    }
    private void SetCrouching(bool value)
    {
        anim.SetBool("isCrouching", value);
    }
    private void SetFalling(bool value)
    {
        anim.SetBool("isFalling", value);
    }
    private void SetJumping(bool value)
    {
        anim.SetBool("isJumping", value);
    }
}
