using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonv2 : MonoBehaviour
{
    [SerializeField] private Element elementRequired;
    [SerializeField] private float delayBeforeOn = 0.5f;
    [SerializeField] GameEvent OnButtonPress;
    [SerializeField] GameEvent OnButtonLeft;

    [SerializeField] List<Element> elementsOnButton = new List<Element>();

    private bool elementIsOn;
    private bool isOff = true;
    private float hitTime;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.TryGetComponent(out DemonElementHandler handler) && !elementsOnButton.Contains(handler.HeldElement))
        {
            elementsOnButton.Add(handler.HeldElement);
            GetElementsOnButton();
            hitTime = Time.time;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.TryGetComponent(out DemonElementHandler handler))
        {
            RemoveDemonFromButton(handler.HeldElement);
        }
    }
    public void OnTransformLeave(DemonElementHandler handler)
    {
        if (elementsOnButton.Contains(handler.HeldElement))
        {
            RemoveDemonFromButton(handler.HeldElement);
        }
    }
    private void RemoveDemonFromButton(Element element)
    {
        elementsOnButton.Remove(element);
        GetElementsOnButton();
    }
    private void GetElementsOnButton()
    {
        foreach(Element element in elementsOnButton)
        {
            if(element == elementRequired) 
            {
                elementIsOn = true;
                return;
            }
        }

        elementIsOn = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown("e")) { elementsOnButton.Add(Element.Earth); GetElementsOnButton(); }
        if (Input.GetKeyDown("f")) { elementsOnButton.Remove(Element.Earth); GetElementsOnButton(); }

        if (elementIsOn && isOff && Time.time - hitTime >= delayBeforeOn)
        {
            if (OnButtonPress != null)
                OnButtonPress.Invoke();
            isOff = false;
        }
        if(!elementIsOn && !isOff)
        {
            print("button is off");
            if (OnButtonLeft != null)
                OnButtonLeft.Invoke();
            isOff = true;
        }
    }
}
