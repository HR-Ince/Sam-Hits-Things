using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] float requiredMass;
    [SerializeField] float turnOffDelay;
    [SerializeField] PlayerStateRegister register;
    [SerializeField] UnityEvent OnPushed;
    [SerializeField] UnityEvent OnLeft;

    private bool buttonLeft;
    private float timeLeft;
    private float massOnButton;
    private List<Rigidbody> bodiesOnButton = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            if (!bodiesOnButton.Contains(rigidbody))
            {
                bodiesOnButton.Add(rigidbody);
                GetMassOnButton();
            }
        }

        print("Mass: " + massOnButton);
        if (massOnButton >= requiredMass)
        {
            if (OnPushed != null)
                OnPushed.Invoke();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            bodiesOnButton.Remove(rigidbody);
            GetMassOnButton();
        }

        if (massOnButton < requiredMass)
        {
            buttonLeft = true;
            timeLeft = Time.time;
        }
            
    }
    private void Update()
    {
        if(buttonLeft && Time.time - timeLeft >= turnOffDelay)
        {
            print("Exit");
            if (OnLeft != null)
                OnLeft.Invoke();
            buttonLeft = false;
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
}
