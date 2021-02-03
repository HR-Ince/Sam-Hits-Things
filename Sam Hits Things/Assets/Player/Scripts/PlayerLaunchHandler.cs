using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunchHandler : MonoBehaviour
{
    [SerializeField] bool allowLaunchInAir = true;
    [SerializeField] float targetMaximumExtension = 10f;
    [SerializeField] WorldStats stats;
    [SerializeField] LayerMask targetableLayer;

    private float drawPercentage;
    private Vector3 dirVector;
    private Vector3 objectPos;
    private Vector3 targetVector;
    private Vector3 pressedPosScreen;
    private Vector3 pressedPosWorld;

    private GroundingCheck grounding;
    private PlayerInput input;
    private Launcher launcher;
    private LineDrawer line;
    private SpriteController sprite;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from PlayerLaunchHandler gameobject"); }
        launcher = GetComponent<Launcher>();
        if (launcher == null) { Debug.LogError("Launcher missing from PlayerLaunchHandler gameobject"); }
        grounding = GetComponentInChildren<GroundingCheck>();
        if(grounding == null) { Debug.LogError("GroundingCheck missing from children of PlayerLaunchHandler gameobject"); }
        line = GetComponentInChildren<LineDrawer>();
        if(line == null) { Debug.LogError("LineDrawer missing from children of PlayerLaunchHandler gameobject"); }
        sprite = GetComponent<SpriteController>();
    }
    private void Update()
    {
        if(!allowLaunchInAir && grounding.GetIsGrounded() == false) { return; }
        if (input.PressInput)
        {
            if (allowLaunchInAir)
                launcher.SetGravity(false);
            SetObjectPos();
            SetPressPos();
        }
        if (input.PressHeldInput)
        {
            BeginTargeting();
            GetDragLength();
            sprite.SortSpriteFacing(dirVector);
            line.SetLinePositions(1, targetVector);
        }
        if (input.PressReleasedInput)
        {
            if (allowLaunchInAir)
                launcher.SetGravity(true);
            launcher.Launch(dirVector, drawPercentage);
            stats.LaunchesMade++;
            line.DisableLine();
            ResetValues();
        }
    }
    private void SetPressPos()
    { 
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
        {
            pressedPosScreen = Input.mousePosition;
            pressedPosWorld = hit.point;
        }
    }
    private void SetObjectPos()
    {
        objectPos = launcher.gameObject.transform.position;
        line.SetFirstPoint(objectPos);
    }
    private void BeginTargeting()
    {
        Vector3 worldTarget = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
            worldTarget = hit.point;

        AdjustTargetting(objectPos, worldTarget);
    }
    private void AdjustTargetting(Vector3 originPos, Vector3 inputPos)
    {
        Vector3 posDif = Vector3.ClampMagnitude(new Vector3(pressedPosWorld.x - inputPos.x, pressedPosWorld.y - inputPos.y), targetMaximumExtension);
        targetVector = originPos + posDif;
        Vector3 aimVector = pressedPosWorld + posDif;

        dirVector = posDif / Vector3.Distance(originPos, targetVector);        
    }
    private void GetDragLength()
    {
        Vector3 inputPos = Input.mousePosition;

        float magnitiude = Vector3.Distance(pressedPosScreen, inputPos);
        float magAsPercent = Mathf.Clamp(magnitiude / (targetMaximumExtension * Screen.dpi) * 100, 0, 100);

        drawPercentage = magAsPercent;
    }
    private void ResetValues()
    {
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
