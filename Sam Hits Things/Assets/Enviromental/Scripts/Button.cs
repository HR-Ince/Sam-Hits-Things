using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] float requiredMass;
    [SerializeField] PlayerStateRegister register;
    [SerializeField] UnityEvent OnPushed;
    [SerializeField] UnityEvent OnLeft;

    private float massOnButton;
    private List<Rigidbody> bodiesOnButton = new List<Rigidbody>();

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            if (GetStopped(rigidbody) && !bodiesOnButton.Contains(rigidbody))
            {
                massOnButton += rigidbody.mass;
                bodiesOnButton.Add(rigidbody);
            }
        }
        print("Mass: " + massOnButton);
        if(massOnButton >= requiredMass)
            if (OnPushed != null)
                OnPushed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            massOnButton -= rigidbody.mass;
            bodiesOnButton.Remove(rigidbody);
        }

        if(massOnButton < requiredMass)
            if (OnLeft != null)
                OnLeft.Invoke();
    }
    private bool GetStopped(Rigidbody other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) { print("No rigidbody in child of " + other.name + " on button"); return false; }

        if (rb.velocity == Vector3.zero)
            return true;
        else
            return false;
    }
}
