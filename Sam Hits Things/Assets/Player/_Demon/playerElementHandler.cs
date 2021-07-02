using UnityEngine;

[RequireComponent(typeof(ElementHolder))]
public class playerElementHandler : MonoBehaviour
{
    [SerializeField] Sprite PH_None, PH_Earth;
    [SerializeField] ElementMasterHandler elementMaster;
    public Element HeldElement { get { return heldElement; } }

    private Element heldElement = Element.None;
    private SpriteRenderer _renderer;
    private Rigidbody rB;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        _renderer = GetComponent<SpriteRenderer>();
        elementMaster.SetDemon(this);
    }

    public void Infuse(Element element)
    {
        heldElement = element;

        if(element == Element.Earth) { _renderer.sprite = PH_Earth; }
        else { _renderer.sprite = PH_None; }
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
