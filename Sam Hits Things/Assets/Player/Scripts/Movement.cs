using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float minimumDrawForLaunch = 10f;
    [SerializeField] float thrustModifier = 10f;

    [SerializeField] MouseData mouseData;
    
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            InputMove();
        }
    }
    public void InputMove()
    {
        if (mouseData.DrawPercentage > minimumDrawForLaunch)
            Move(mouseData.DirectionVector * (mouseData.DrawPercentage * thrustModifier));
        else
            Debug.Log("Draw under minimum");
    }

    private void Move(Vector2 force)
    {
        rigidBody.AddForce(force, ForceMode.Impulse);
    }
}
