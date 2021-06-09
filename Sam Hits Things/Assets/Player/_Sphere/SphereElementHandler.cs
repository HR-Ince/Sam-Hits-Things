using UnityEngine;

[RequireComponent(typeof(ElementHolder))]
public class SphereElementHandler : MonoBehaviour
{
    public Element HeldElement { get { return heldElement; } }

    private Element heldElement;
    private Rigidbody rB;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }

    public void Infuse(Element element)
    {
        heldElement = element;
    }

    public void ActivateWind()
    {
        rB.useGravity = false;
    }

    public void clearElement()
    {
        rB.useGravity = true;
    }
}
