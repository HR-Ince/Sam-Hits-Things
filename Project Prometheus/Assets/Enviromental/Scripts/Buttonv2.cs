using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonv2 : MonoBehaviour
{
    [SerializeField] private float massRequired = 0.75f;
    [SerializeField] private float delayBeforeOn = 0.5f;
    [SerializeField] GameEvent OnButtonPress;
    [SerializeField] GameEvent OnButtonLeft;

    List<Rigidbody> bodiesOnButton = new List<Rigidbody>();

    private bool isOff = true;
    private float hitTime;
    private float massOnButton;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Rigidbody rigidbody) && !bodiesOnButton.Contains(rigidbody))
        {
            bodiesOnButton.Add(rigidbody);
            GetMassOnButton();
            hitTime = Time.time;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            bodiesOnButton.Remove(rigidbody);
            GetMassOnButton();
        }
    }
    private void GetMassOnButton()
    {
        massOnButton = 0;

        foreach(Rigidbody rigidbody in bodiesOnButton)
        {
            massOnButton += rigidbody.mass;
        }
    }
    private void Update()
    {
        if (massOnButton >= massRequired && Time.time - hitTime >= delayBeforeOn && isOff)
        {
            if (OnButtonPress != null)
                OnButtonPress.Invoke();
            isOff = false;
        }
        if(massOnButton < massRequired && !isOff)
        {
            if (OnButtonLeft != null)
                OnButtonLeft.Invoke();
            isOff = true;
        }
    }
}
