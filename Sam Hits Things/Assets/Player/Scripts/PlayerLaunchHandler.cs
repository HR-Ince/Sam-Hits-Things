using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerLaunchHandler : MonoBehaviour
{
    [SerializeField] bool allowLaunchInAir = true;
    [SerializeField] float targetMaximumExtension = 10f;
    [SerializeField] WorldStats stats;

    private float cameraDepth;
    private float drawPercentage;
    private GameObject target;
    private Vector3 dirVector;
    private Vector3 objectPos;
    private Vector3 targetVector;
    private Vector3 pressedPosWorld;

    private GroundingCheck grounding;
    private PlayerInput input;
    private Movement launcher;
    private LineDrawer line;
    private SpriteController spriteController;

    private void Awake()
    {
        launcher = GetComponent<Movement>();

        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from PlayerLaunchHandler gameobject"); }
        
        spriteController = GetComponentInChildren<SpriteController>();
        if(spriteController == null) { Debug.LogError("SpriteController missing from PlayerLaunchHandler gameobject"); }
        grounding = GetComponentInChildren<GroundingCheck>();
        if(grounding == null) { Debug.LogError("GroundingCheck missing from children of PlayerLaunchHandler gameobject"); }
        line = GetComponentInChildren<LineDrawer>();
        if(line == null) { Debug.LogError("LineDrawer missing from children of PlayerLaunchHandler gameobject"); }

        cameraDepth = -Camera.main.transform.position.z;

        target = launcher.gameObject;
    }
    private void Update()
    {
        if(!allowLaunchInAir && !grounding.IsGrounded) { return; }
        if (input.Pressed)
        {
            if (allowLaunchInAir)
                launcher.SetUseGravity(false);
            SetObjectPos();
            SetPressPos();
        }
        if (input.PressHeld)
        {
            Vector3 inputPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
            AdjustTargetting(inputPosWorld);
            GetDragLength(inputPosWorld);
            spriteController.SortSpriteFacing(dirVector);
            line.SetLinePositions(targetVector, drawPercentage);
        }
        if (input.PressReleased)
        {
            if (allowLaunchInAir)
                launcher.SetUseGravity(true);
            if(launcher.Launch(dirVector, drawPercentage))
                stats.LaunchesMade++;
            line.DisableLine();
            ResetValues();
        }
    }
    private void SetPressPos()
    {
        pressedPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
    }
    private void SetObjectPos()
    {
        objectPos = target.transform.position;
        line.SetFirstPoint(objectPos);
    }
    private void AdjustTargetting(Vector3 inputPosWorld)
    {
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPosWorld, targetMaximumExtension);
        targetVector = target.transform.position + posDif;
        dirVector = posDif / Vector3.Distance(target.transform.position, targetVector);

        if (grounding.IsGrounded)
        {
            dirVector = new Vector3(dirVector.x, Mathf.Max(dirVector.y, 0.00f));
            targetVector = new Vector3(targetVector.x, Mathf.Max(target.transform.position.y, targetVector.y));
        }

    }
    private void GetDragLength(Vector3 inputPosWorld)
    {
        float magnitiude = Vector3.Distance(pressedPosWorld, inputPosWorld);

        drawPercentage = Mathf.Clamp(magnitiude / targetMaximumExtension * 100, 0, 100);
    }
    private void ResetValues()
    {
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
