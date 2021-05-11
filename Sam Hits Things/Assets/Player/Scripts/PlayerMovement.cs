using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] bool wallJumpEnabled = false;
    [SerializeField] float forceOnSpring = 0.5f;

    private Vector3 directionOfTravel;
    private float drawPercentageApplied;

    private Rigidbody rigidBody;
    private ManualLauncher launcher;
    private OriballPlayerController controller;

    private void Awake()
    {
        FetchExternalVariables();
    }
    private void FetchExternalVariables()
    {
        rigidBody = GetComponent<Rigidbody>();
        controller = GetComponent<OriballPlayerController>();
        if (controller == null) { Debug.LogError("PlayerController missing from Player"); }
        launcher = GetComponent<ManualLauncher>();
        if (launcher == null) { Debug.LogError("ManualLanucher missing from Player"); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (wallJumpEnabled && !controller.GetIsGroundedNarrow())
            Wallspring();
    }
    public void SetLaunchVariables()
    {        
        directionOfTravel = launcher.DirectionVector;
        drawPercentageApplied = launcher.DrawPercentage;
    }
    private void Wallspring()
    {
        Vector3 springDir = new Vector3(-directionOfTravel.x, directionOfTravel.y);
        float thrust = drawPercentageApplied * forceOnSpring;
        rigidBody.AddForce(springDir * thrust);
    }
}
