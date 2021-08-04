using UnityEngine;

public class DemonElementHandler : MonoBehaviour
{
    [SerializeField] float earthFormMass;
    [SerializeField] Sprite defaultSprite, earthSprite;

    public bool IsHoldingElement { get { return heldElement != Element.None; } }
    public Element HeldElement { get { return heldElement; } }

    private float defaultMass;

    private Element heldElement = Element.None;
    private DemonAirFormMod airMod;
    private SpriteRenderer _renderer;
    private Rigidbody rB;

    private void Awake()
    {
        FetchExternalVariables();

        airMod.enabled = false;
        defaultMass = rB.mass;
    }

    private void FetchExternalVariables()
    {
        rB = GetComponent<Rigidbody>();
        _renderer = GetComponent<SpriteRenderer>();
        airMod = GetComponent<DemonAirFormMod>();
    }

    public void Infuse(Element element)
    {
        heldElement = element;
        _renderer.sprite = ElementToSprite(element);
        ActivateElement(element);
    }

    private Sprite ElementToSprite(Element element)
    {
        if (element == Element.Earth) { return earthSprite; }
        else { return defaultSprite; }
    }

    private void ActivateElement(Element element)
    {
        if(element == Element.Earth) { ActivateEarth(); }
        else { ClearElement(); }
    }

    private void ActivateEarth()
    {
        rB.mass = earthFormMass;
    }

    private void ActivateWind()
    {
        airMod.enabled = true;
    }

    public void ClearElement()
    {
        airMod.enabled = false;
        rB.mass = defaultMass;
    }
}
