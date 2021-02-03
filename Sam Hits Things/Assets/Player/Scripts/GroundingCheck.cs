using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingCheck : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    private bool isGrounded;
    private Collider groundingBox;

    private void Awake()
    {
        groundingBox = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == groundLayer)
            isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;        
    }
    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}
