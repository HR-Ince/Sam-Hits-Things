using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingCheck : MonoBehaviour
{
    [SerializeField] float slipForce;
    [SerializeField] WorldState world;

    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponentInParent<Rigidbody>();
    }
    private void Update()
    {
        if(!isGrounded && body.velocity.y == 0)
        {
            extraPush();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == world.FloorLayer)
        {
            isGrounded = true;
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;        
    }
    private void extraPush()
    {
        Physics.Raycast(transform.position, Vector3.left, out RaycastHit hitLeft, 5f, world.FloorLayer);
        Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitRight, 5f, world.FloorLayer);

        if (Vector3.Distance(transform.position, hitLeft.point) > Vector3.Distance(transform.position, hitRight.point))
        {
            body.AddForce(new Vector3(slipForce, 0));
        }
        else
        {
            body.AddForce(new Vector3(-slipForce, 0));
        }
    }
}
