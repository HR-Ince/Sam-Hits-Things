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
        if (GetStopped(other))
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
    private bool GetStopped(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) { print("No rigidbody in child of " + other.name + " on button"); return false; }

        if (rb.velocity == Vector3.zero)
            return true;
            
        else
            return false;
    }
}
