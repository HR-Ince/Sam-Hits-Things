using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class BoarPlayerController : PlayerControllerBase
{
    private bool isBurdened = false;
    private bool pressIsGood;
    private GameObject heldObject;

    private BoxCollider boxCollider;
    private PlayerGrabHandler grabHandler;
    private Rigidbody heldRigidbody;

    private void Awake()
    {
        FetchExternalVariables();

        colliderHalfExtents = new Vector3(boxCollider.size.x / 2, boxCollider.size.y / 2, boxCollider.size.z / 2);
        colliderCentre = boxCollider.center;
    }
    private new void FetchExternalVariables()
    {
        base.FetchExternalVariables();
        grabHandler = GetComponentInChildren<PlayerGrabHandler>();
        if (grabHandler == null) { Debug.LogError("GrabHandler missing from children of player"); }
        if(grabHandler.transform.position == transform.position) { Debug.Log("Grab handler not offset from player"); }
        boxCollider = _collider as BoxCollider;
    }

    private void Update()
    {
        GetIsGrounded();
        ManageBurden();

        if (isGrounded)
        {
            if (input.Pressed)
            {
                ProcessPress();
            }

            if (input.PressHeld && pressIsGood)
            {
                ProcessPressHeld();
            }

            if (input.PressReleased && pressIsGood)
            {
                ProcessPressReleased();
            }

            if (!isBurdened)
            {
                grabHandler.ManageCollectableObjects();
            }
            else
            {
                grabHandler.DisableLabels();
            }
        }
        else
        {
            grabHandler.DisableLabels();
        }
    }

    private void ProcessPressReleased()
    {
        line.DisableLine();
        if (targetting.DrawIsSufficient())
        {
            if (isBurdened)
            {
                grabHandler.ReleaseHeldObject();
                launcher.Launch(heldRigidbody, targetting.DirectionVector, targetting.DrawPercentage, jumpStrength, ForceMode.Force);
                heldObject = null;
            }
            else
            {
                launcher.Launch(_rigidbody, targetting.DirectionVector, targetting.DrawPercentage, jumpStrength, ForceMode.Force);
            }
        }

        pressIsGood = false;
    }

    private void ProcessPressHeld()
    {
        targetting.AdjustTargetting(input.PressPos);
        if (targetting.DrawIsSufficient())
        {
            if (isBurdened)
                line.ManageTrajectoryLine(jumpStrength, 0, heldRigidbody.mass, ForceMode.Force); // Additional thrust variable needed
            else
                line.ManageTrajectoryLine(jumpStrength, 0, _rigidbody.mass, ForceMode.Force);
            line.enabled = true;
        }
        else
            line.enabled = false;
        SortFacing(targetting.DirectionVector);
    }

    private new void ProcessPress()
    {
        if (isBurdened)
        {
            targetting.SetupTargetting(heldObject.transform.position, input.PressPos);
            pressIsGood = true;
        }
        else if (!isBurdened && grabHandler.CollectableObjectPressed())
        {
            grabHandler.Grab();
            heldObject = grabHandler.HeldObject;
            grabHandler.DisableLabels();
            return;
        }
        else
        {
            base.ProcessPress();
        }
        SortFacing(input.PressPos - transform.position);
    }

    private void ManageBurden()
    {
        isBurdened = grabHandler.HeldObject != null;
        if (isBurdened && grabHandler.HeldObject.TryGetComponent(out Rigidbody rigidbody))
            heldRigidbody = rigidbody;
    }
}
