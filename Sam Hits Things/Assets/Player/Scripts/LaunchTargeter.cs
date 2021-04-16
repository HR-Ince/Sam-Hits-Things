using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class LaunchTargeter : MonoBehaviour
{
    [SerializeField] float targetMaximumExtension = 10f, minimumDrawPercentage = 5f;
    [SerializeField] UnityEvent onJump;

    public Vector3 DirectionVector { get { return dirVector; } }
    public Vector3 TargetVector { get { return targetVector; } }
    public float DrawPercentage { get { return drawPercentage; } }
    public float TargetMaxExtension { get { return targetMaximumExtension; } }

    private bool allowLaunchInAir;
    private bool pressAccepted;
    private float cameraDepth;
    private float drawPercentage;
    private GameObject target;
    private Vector3 dirVector;
    private Vector3 targetVector;
    private Vector3 pressedPosWorld;

    private PlayerInput input;
    private PlayerMovement player;
    private LineDrawer line;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();

        input = GetComponent<PlayerInput>();
        if(input == null) { Debug.LogError("PlayerInput missing from PlayerLaunchHandler gameobject"); }
        line = GetComponentInChildren<LineDrawer>();
        if(line == null) { Debug.LogError("LineDrawer missing from children of PlayerLaunchHandler gameobject"); }

        cameraDepth = -Camera.main.transform.position.z;

        target = player.gameObject;
        allowLaunchInAir = player.AllowLaunchInAir;
    }
    private void Update()
    {
        if(!allowLaunchInAir && !player.GetIsStopped()) { return; }
        if (input.Pressed)
        {
            if (allowLaunchInAir)
                player.SetUseGravity(false);
            SetPressPos();
            pressAccepted = true;
        }
        if (input.PressHeld && pressAccepted)
        {
            Vector3 inputPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
            AdjustTargetting(inputPosWorld);
            GetDragLength(inputPosWorld);
            player.SortFacing(dirVector);
            PassLineValues();
        }
        if (input.PressReleased && pressAccepted)
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
        pressedPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(input.PressPos.x, input.PressPos.y, cameraDepth));
    }
    private void AdjustTargetting(Vector3 inputPosWorld)
    {
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPosWorld, targetMaximumExtension);
        targetVector = target.transform.position + posDif;
        dirVector = posDif / Vector3.Distance(target.transform.position, targetVector);

        dirVector = new Vector3(dirVector.x, Mathf.Max(dirVector.y, 0.00f));
        targetVector = new Vector3(targetVector.x, Mathf.Max(target.transform.position.y, targetVector.y));

    }
    private void GetDragLength(Vector3 inputPosWorld)
    {
        float magnitiude = Vector3.Distance(pressedPosWorld, inputPosWorld);
        drawPercentage = Mathf.Clamp(magnitiude / targetMaximumExtension * 100, 0, 100);
    }
    private void ResetValues()
    {
        pressAccepted = false;
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
