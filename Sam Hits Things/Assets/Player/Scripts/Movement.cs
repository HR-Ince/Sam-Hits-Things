using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] MouseData mouseData;

    Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InputMove()
    {
        Move(transform.up * (mouseData.DragLength * 10));
    }

    private void Move(Vector2 force)
    {
        rigidBody.AddForce(force);
    }
}
