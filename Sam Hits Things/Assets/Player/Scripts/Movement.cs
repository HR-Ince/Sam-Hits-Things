using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] MouseData mouseData;
    [SerializeField] float thrustModifier = 10f;

    Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InputMove()
    {        
        Vector2 transPos2 = new Vector2(transform.position.x, transform.position.y);
        Vector2 launchVector = (mouseData.ClickPos - transPos2) / Vector2.Distance(transPos2, mouseData.ClickPos);

        Move(launchVector * (mouseData.DragLength * thrustModifier));
    }

    private void Move(Vector2 force)
    {
        print(force);
        rigidBody.AddForce(force);
    }
}
