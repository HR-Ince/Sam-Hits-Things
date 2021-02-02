using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunchHandler : MonoBehaviour
{
    [SerializeField] float targetMaximumExtension = 10f;
    [SerializeField] WorldStats stats;

    [SerializeField] GameObject posKnob, targetKnob, worldKnob;

    [SerializeField] LayerMask targetableLayer;

    private float drawPercentage;
    private Vector3 dirVector;
    private Vector3 objectPos;
    private Vector3 pressedPosScreen;
    private Vector3 pressedPosWorld;

    private PlayerInput input;
    private Launcher launcher;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from PlayerLaunchHandler gameobject"); }
        launcher = GetComponent<Launcher>();
        if (launcher == null) { Debug.LogError("Launcher missing from PlayerLaunchHandler gameobject"); }
    }
    private void Update()
    {
        if (input.PressInput)
        {
            SetObjectPos();
            SetPressPos();
        }
        if (input.PressHeldInput)
        {
            BeginTargeting();
            GetDragLength();
        }
        if (input.PressReleasedInput)
        {
            launcher.Launch(dirVector, drawPercentage);
            stats.LaunchesMade++;
            ResetValues();
        }
    }

    public void SetPressPos()
    { 
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
        {
            pressedPosScreen = Input.mousePosition;
            pressedPosWorld = hit.point;
            posKnob.transform.position = hit.point;
        }
    }
    public void SetObjectPos()
    {
        objectPos = launcher.gameObject.transform.position;
    }
    public void BeginTargeting()
    {
        Vector3 worldTarget = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
            worldTarget = hit.point;

        AdjustTargetting(objectPos, worldTarget);
    }
    private void AdjustTargetting(Vector3 originPos, Vector3 inputPos)
    {
        Vector3 posDif = Vector3.ClampMagnitude(new Vector3(pressedPosWorld.x - inputPos.x, pressedPosWorld.y - inputPos.y), targetMaximumExtension);
        Vector2 targetVector = originPos + posDif;
        Vector3 aimVector = pressedPosWorld + posDif;

        dirVector = posDif / Vector3.Distance(originPos, targetVector);
        
        targetKnob.transform.position = aimVector;
        worldKnob.transform.position = targetVector;
        
    }
    public void GetDragLength()
    {
        Vector3 inputPos = Input.mousePosition;

        float magnitiude = Vector3.Distance(pressedPosScreen, inputPos);
        float magAsPercent = Mathf.Clamp(magnitiude / (targetMaximumExtension * Screen.dpi) * 100, 0, 100);

        drawPercentage = magAsPercent;
    }
    public void ResetValues()
    {
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
