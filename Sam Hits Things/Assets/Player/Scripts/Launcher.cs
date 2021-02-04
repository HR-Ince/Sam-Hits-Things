using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Launcher : MonoBehaviour
{
    [SerializeField] float minimumDrawForLaunch = 10f;
    [SerializeField] float thrustModifier = 10f;
    
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    public void SetUseGravity(bool useGravity)
    {
        if(useGravity == false)
            rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = useGravity;
    }
    public void Launch(Vector2 dirVector, float drawPercentage)
    {
        float thrust = drawPercentage * thrustModifier;
        if (drawPercentage > minimumDrawForLaunch)
            rigidBody.AddForce(dirVector * thrust);
        else
            Debug.Log("Draw under minimum");
    }
}
