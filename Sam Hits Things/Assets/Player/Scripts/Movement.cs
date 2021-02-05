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
    [SerializeField] float minimumDrawForLaunch = 10f;
    
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
    public bool Launch(Vector2 dirVector, float drawPercentage)
    {
        float thrust = drawPercentage * thrustModifier;
        if (drawPercentage > minimumDrawForLaunch)
        {
            rigidBody.AddForce(dirVector * thrust);
            appliedForce = dirVector * thrust;
            return true;
        }
        else
        {
            Debug.Log("Draw under minimum");
            return false;
        }        
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
