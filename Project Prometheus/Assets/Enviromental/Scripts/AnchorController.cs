using UnityEngine;

public class AnchorController : ElementVesselController
{
    [SerializeField] Element heldElement;
    [SerializeField] protected Sprite noneSprite, earthSprite;

    public Element HeldElement { get { return heldElement; } }

    private DemonElementHandler myDemon;

    public void SetDemon(DemonElementHandler demon) { myDemon = demon; }

    public override ElementVesselController ReportSelf()
    {
        return this;
    }

    protected override void UpdateSprite()
    {
        if (heldElement == Element.Earth) { _renderer.sprite = earthSprite; }
        else { _renderer.sprite = noneSprite; }

    }
    public override bool CanReceiveElement()
    { return myDemon.IsHoldingElement; }
    public override bool CanGiveElement()
    { return heldElement != Element.None; }
    public override void ReceiveElement(Element element)
    { 
        heldElement = element;
        UpdateSprite();
    }
}
