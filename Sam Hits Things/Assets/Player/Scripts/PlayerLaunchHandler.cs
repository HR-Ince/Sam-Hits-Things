using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Launcher))]
public class PlayerLaunchHandler : MonoBehaviour
{
    [SerializeField] bool allowLaunchInAir = true;
    [SerializeField] float targetMaximumExtension = 10f;
    [SerializeField] float cameraClippingPlane = 10f;
    [SerializeField] WorldStats stats;
    [SerializeField] LayerMask targetableLayer;

    private float drawPercentage;
    private GameObject target;
    private Vector3 dirVector;
    private Vector3 objectPos;
    private Vector3 targetVector;
    private Vector3 pressedPosWorld;

    private GroundingCheck grounding;
    private PlayerInput input;
    private Launcher launcher;
    private LineDrawer line;
    private SpriteController sprite;

    private void Awake()
    {
        launcher = GetComponent<Launcher>();

        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from PlayerLaunchHandler gameobject"); }
        grounding = GetComponentInChildren<GroundingCheck>();
        if(grounding == null) { Debug.LogError("GroundingCheck missing from children of PlayerLaunchHandler gameobject"); }
        line = GetComponentInChildren<LineDrawer>();
        if(line == null) { Debug.LogError("LineDrawer missing from children of PlayerLaunchHandler gameobject"); }
        sprite = GetComponent<SpriteController>();

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
            Vector3 worldInputPos = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraClippingPlane));
            AdjustTargetting(worldInputPos);
            GetDragLength(worldInputPos);
            sprite.SortSpriteFacing(dirVector);
            line.SetLinePositions(targetVector, drawPercentage);
        }
        if (input.PressReleased)
        {
            if (allowLaunchInAir)
                launcher.SetUseGravity(true);
            launcher.Launch(dirVector, drawPercentage);
            stats.LaunchesMade++;
            line.DisableLine();
            ResetValues();
        }
    }
    private void SetPressPos()
    {
        pressedPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraClippingPlane));
    }
    private void SetObjectPos()
    {
        objectPos = target.transform.position;
        line.SetFirstPoint(objectPos);
    }
    private void AdjustTargetting(Vector3 inputPos)
    {
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPos, targetMaximumExtension);
        targetVector = target.transform.position + posDif;
        dirVector = posDif / Vector3.Distance(target.transform.position, targetVector);

        if (grounding.IsGrounded)
        {
            dirVector = new Vector3(dirVector.x, Mathf.Max(dirVector.y, 0.00f));
            targetVector = new Vector3(targetVector.x, Mathf.Max(target.transform.position.y, targetVector.y));
        }

    }
    private void GetDragLength(Vector3 inputPos)
    {
        float magnitiude = Vector3.Distance(pressedPosWorld, inputPos);
                
        float magAsPercent = Mathf.Clamp(magnitiude / targetMaximumExtension * 100, 0, 100);
        drawPercentage = magAsPercent;

        print(magAsPercent);
    }
    private void ResetValues()
    {
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
