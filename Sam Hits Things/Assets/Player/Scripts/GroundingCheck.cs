using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingCheck : MonoBehaviour
{
    [SerializeField] int groundLayerNumber;

    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == groundLayerNumber)
        {
            isGrounded = true;
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;        
    }
}
