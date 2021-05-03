using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class LaunchTargeter : MonoBehaviour
{
    [SerializeField] float maxMagnitude = 10f, minimumDrawPercentage = 5f;
    [SerializeField] UnityEvent onJump;

    public Vector3 DirectionVector { get { return dirVector; } }
    public Vector3 TargetVector { get { return targetVector; } }
    public float DrawPercentage { get { return drawPercentage; } }
    public float TargetMaxExtension { get { return maxMagnitude; } }

    private bool allowLaunchInAir;
    private bool pressAccepted;
    private float cameraDepth;
    private float drawPercentage;
    private GameObject target;
    private Vector3 dirVector;
    private Vector3 targetVector;
    private Vector3 pressedPosWorld;

    private Camera cam;
    private PlayerInput input;
    private PlayerMovement player;
    private PlayerStateManager state;
    private LineDrawer line;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        cam = Camera.main;

        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from LaunchTargeter"); }
        line = GetComponentInChildren<LineDrawer>();
        if(line == null) { Debug.LogError("LineDrawer missing from children of LaunchTargeter"); }
        state = GetComponent<PlayerStateManager>();
        if(state == null) { Debug.LogError("State manager missing from LaunchTargeter"); }

        cameraDepth = -Camera.main.transform.position.z;

        target = player.gameObject;
        allowLaunchInAir = player.AllowLaunchInAir;
    }
    private void Update()
    {
        if(!allowLaunchInAir && !state.IsGrounded) { return; }
        if (input.Pressed)
        {
            if (allowLaunchInAir)
                player.SetUseGravity(false);
            SetPressPos();
            pressAccepted = true;
        }
        if (pressAccepted && input.PressHeld)
        {
            AdjustTargetting();
            player.SortFacing(dirVector);
            PassLineValues();
        }
        if (pressAccepted && input.PressReleased)
        {
            if (allowLaunchInAir)
                player.SetUseGravity(true);
            if (drawPercentage > minimumDrawPercentage)
            {
                if (onJump != null)
                    onJump.Invoke();
                player.Launch(dirVector, drawPercentage);
            }
            else { Debug.Log("Draw under minimum"); }
            DisableLine();
            ResetValues();
        }

    }
    private void DisableLine()
    {
        if(line == null) { return; }
        line.DisableLine();
    }
    private void PassLineValues()
    {
        if(line == null) { return; }
        line.SetLinePositions(transform.position, targetVector);
        line.SetLineColour(drawPercentage);
    }
    private void SetPressPos()
    {
        pressedPosWorld = cam.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
    }
    private void AdjustTargetting()
    {
        Vector3 inputPosWorld = cam.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPosWorld, maxMagnitude);
        targetVector = target.transform.position + posDif;
        targetVector = new Vector3(targetVector.x, Mathf.Max(target.transform.position.y, targetVector.y));

        float difMagnitude = Mathf.Abs(Mathf.Clamp(Vector3.Distance(pressedPosWorld, inputPosWorld), 0, maxMagnitude));
        dirVector = posDif / difMagnitude;
        drawPercentage = Mathf.Clamp(difMagnitude / maxMagnitude * 100, 0, 100);
    }
    private void ResetValues()
    {
        pressAccepted = false;
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
