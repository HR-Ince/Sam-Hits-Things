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

    private Vector3 appliedForce;
    private Vector3 directionOfTravel;
    private float drawPercentageApplied;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (wallJumpEnabled && collision.gameObject.layer == world.WallLayer)
            Wallspring();
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
            mesh.transform.rotation = Quaternion.Euler(-90, 0, 90);
        else
        {
            mesh.transform.rotation = Quaternion.Euler(-90, 0, 270);
        }

    }
    public void Launch(Vector2 direction, float drawPercentage)
    {
        directionOfTravel = direction;
        drawPercentageApplied = drawPercentage;
        float thrust = drawPercentage * thrustModifier;
        rigidBody.AddForce(direction * thrust);
        appliedForce = direction * thrust;
    }
    private void Wallspring()
    {
        Vector3 springDir = new Vector3(-directionOfTravel.x, directionOfTravel.y);
        print(springDir);
        Launch(springDir, drawPercentageApplied * percentageForceOnSpring);
        SortFacing(springDir);
    }
    public void Stop()
    {
        rigidBody.velocity = Vector3.zero;
    }
}
