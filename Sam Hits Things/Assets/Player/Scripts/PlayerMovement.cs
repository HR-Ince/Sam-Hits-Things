using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] bool allowLaunchInAir = true;
    [SerializeField] bool wallJumpEnabled = false;
    [SerializeField] WorldState world;

    [SerializeField] float thrustModifier = 10f;
    [SerializeField] float percentageForceOnSpring = 0.5f;
    [SerializeField] float burdenedThrustPercentage = 0.6f;

    public bool AllowLaunchInAir { get { return allowLaunchInAir; } }

    private Vector3 directionOfTravel;
    private float drawPercentageApplied;

    private Rigidbody rigidBody;
    private PlayerStateManager state;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        state = GetComponent<PlayerStateManager>();
        if(state == null) { Debug.LogError("State manager missing from Player"); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (wallJumpEnabled && collision.gameObject.layer == world.WallLayer)
            Wallspring();
    }
    private void FixedUpdate()
    {
        if (rigidBody.velocity == Vector3.zero)
        {
            state.IsStopped = true;
        }
        else
        {
            state.IsStopped = false;
        }
    }
    public void SetUseGravity(bool useGravity)
    {
        if(useGravity == false)
            rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = useGravity;
    }
    public void SortFacing(Vector3 direction)
    {
        if (direction.x > 0.00f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void Launch(Vector2 direction, float drawPercentage)
    {
        if (state.IsBurdened)
            drawPercentage = drawPercentage * burdenedThrustPercentage;
        directionOfTravel = direction;
        drawPercentageApplied = drawPercentage;
        float thrust = drawPercentage * thrustModifier;
        rigidBody.AddForce(direction * thrust);
    }
    private void Wallspring()
    {
        Vector3 springDir = new Vector3(-directionOfTravel.x, directionOfTravel.y);
        Launch(springDir, drawPercentageApplied * percentageForceOnSpring);
        SortFacing(springDir);
    }
}
