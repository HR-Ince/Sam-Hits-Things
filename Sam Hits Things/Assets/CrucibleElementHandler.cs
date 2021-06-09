using UnityEngine;

public class CrucibleElementHandler : MonoBehaviour
{
    [SerializeField] Element heldElement;
    [SerializeField] CrucibleDataHandler data;
    
    public Element HeldElement { get { return heldElement; } }

    public void OnSphereDetection()
    {
        data.SetElementFrom(this, heldElement);
    }

    public void SetElement(Element element)
    {
        heldElement = element;
    }

    public void onSphereLeaving()
    {
        data.ClearActiveCrucible();
    }
}
