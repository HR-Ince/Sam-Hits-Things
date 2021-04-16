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

    [SerializeField] GameObject mesh;
    
    public bool AllowLaunchInAir { get { return allowLaunchInAir; } }

    private bool hasJumped = false;
    private Vector3 directionOfTravel;
    private float drawPercentageApplied;
    private Vector3 launchStart, launchEnd;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!GetIsStopped())
        {
            hasJumped = true;
        }
        if (GetIsStopped() && hasJumped)
        {
            launchEnd = transform.position;
            string distance = Vector3.Distance(launchStart, launchEnd).ToString("F2");
            print("Distance travelled: " + distance);
            hasJumped = false;
        }
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (wallJumpEnabled && collision.gameObject.layer == world.WallLayer)
            Wallspring();
    }
    public bool GetIsStopped()
    {
        if (rigidBody.velocity == Vector3.zero)
            return true;
        else
            return false;
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
        launchStart = transform.position;
        directionOfTravel = direction;
        drawPercentageApplied = drawPercentage;
        float thrust = drawPercentage * thrustModifier;
        rigidBody.AddForce(direction * thrust);
        string launchAngle = Vector3.Angle(Vector3.right, direction).ToString("F2");
        print("Launch angle: " + launchAngle);
    }
    private void Wallspring()
    {
        Vector3 springDir = new Vector3(-directionOfTravel.x, directionOfTravel.y);
        Launch(springDir, drawPercentageApplied * percentageForceOnSpring);
        SortFacing(springDir);
    }
    public void Stop()
    {
        rigidBody.velocity = Vector3.zero;
    }
}
