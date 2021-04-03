using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] bool wallJumpEnabled = false;
    [SerializeField] float wallLayer;

    [SerializeField] float springForce = 200f;
    [SerializeField] float thrustModifier = 10f;
    [SerializeField] float percentageForceOnSpring = 0.4f;

    [SerializeField] GameObject mesh;
    
    private Vector3 appliedForce;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(wallJumpEnabled && collision.gameObject.layer == wallLayer)
            Spring();
    }
    public void SetUseGravity(bool useGravity)
    {
        if(useGravity == false)
            rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = useGravity;
    }

    public void SortFacing(Vector3 direction)
    {
        if (direction.x > 0)
            mesh.transform.rotation = Quaternion.Euler(-90, 0, 90);
        else
        {
            mesh.transform.rotation = Quaternion.Euler(-90, 0, 270);
        }

    }
    public void Launch(Vector2 dirVector, float drawPercentage)
    {
        float thrust = drawPercentage * thrustModifier;
        rigidBody.AddForce(dirVector * thrust);
        appliedForce = dirVector * thrust;
    }
    private void Spring()
    {
        Vector3 springForceVector = new Vector3(-appliedForce.x * percentageForceOnSpring, springForce);
        rigidBody.AddForce(springForceVector);
    }
    public void Stop()
    {
        rigidBody.velocity = Vector3.zero;
    }
}
