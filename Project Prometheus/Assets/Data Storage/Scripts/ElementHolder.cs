using UnityEngine;

public class ElementHolder : MonoBehaviour
{
    public Element HeldElement { get { return heldElement; } set { heldElement = value; } }
    [SerializeField] Element heldElement;
}
