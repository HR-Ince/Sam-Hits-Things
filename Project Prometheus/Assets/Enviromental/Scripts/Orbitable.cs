using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Orbitable : MonoBehaviour
{
    [SerializeField] float recollisionDelay = 0.4f;
    [SerializeField] PlayerStateRegister register;

    private Collider _collider;
    private Collider playerCollider;
    private float startTime;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        playerCollider = register.PlayerOne.GetComponent<Collider>();
    }

    private void Update()
    {
        ManageCollisionIgnorance();
    }

    private void ManageCollisionIgnorance()
    {
        if (Physics.GetIgnoreCollision(_collider, playerCollider))
        {
            if (startTime == 0f)
                startTime = Time.time;

            if (Time.time - startTime >= recollisionDelay && !PlayerIsInCollider())
            {
                Physics.IgnoreCollision(_collider, playerCollider, false);
                startTime = 0f;
            }
        }
    }

    private bool PlayerIsInCollider()
    {
        if (_collider is SphereCollider sphereColl)
        {
            Collider[] interiorObjects = Physics.OverlapSphere(transform.position, sphereColl.radius);

            foreach (Collider collider in interiorObjects)
            {
                if (collider == playerCollider)
                {
                    return true;
                }
                    
            }
        }
        else if (_collider is BoxCollider boxColl)
        {
            Collider[] interiorObjects = Physics.OverlapBox(transform.position, boxColl.size / 2);

            foreach (Collider collider in interiorObjects)
            {
                if (collider == playerCollider)
                {
                    return true;
                }
            }
        }
        else if (_collider is CapsuleCollider capColl)
        {
            Vector3 point0 = new Vector3(transform.position.x, transform.position.y + capColl.height / 2, transform.position.z);
            Vector3 point1 = new Vector3(transform.position.x, transform.position.y - capColl.height / 2, transform.position.z);
            Collider[] interiorObjects = Physics.OverlapCapsule(point0, point1, capColl.radius);

            foreach (Collider collider in interiorObjects)
            {
                if (collider == playerCollider)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
