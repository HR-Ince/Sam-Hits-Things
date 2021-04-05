using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] PlayerRegister register;
    [SerializeField] UnityEvent OnPushed;
    [SerializeField] UnityEvent OnLeft;

    private void OnTriggerStay(Collider other)
    {
        if (GetGrounded(other))
        {
            if (OnPushed != null)
                OnPushed.Invoke();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnLeft != null)
            OnLeft.Invoke();
    }
    private bool GetGrounded(Collider other)
    {
        GroundingCheck gC = other.transform.parent.GetComponentInChildren<GroundingCheck>();
        if (gC == null) { print("No GroundCheck in " + other.transform.parent.name + " on button"); return false; }

        if (gC.IsGrounded)
            return true;
        else
            return false;
    }
}
