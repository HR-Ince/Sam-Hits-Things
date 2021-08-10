using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : ElementVesselController
{
    [SerializeField] List<Element> requiredElements;

    [SerializeField] private List<Element> heldElements;

    private ChaplainElementHandler chaplain;

    private new void Awake()
    {
        base.Awake();
        heldElements = new List<Element>(requiredElements.Count);
        chaplain = FindObjectOfType<ChaplainElementHandler>();
    }

    public override ElementVesselController ReportSelf()
    {
        return this;
    }

    protected override void UpdateSprite()
    {
        //Change sprite
    }

    public override bool CanReceiveElement()
    {
        foreach(Element element in chaplain.HeldElements)
        {
            if (!heldElements.Contains(element) || requiredElements.Contains(element)) { return true; }
            else { return false; }
        }

        return false;
    }
    public override bool CanGiveElement()
    { return false; }
    private void CheckElements()
    {
        if(heldElements.Count != requiredElements.Count) { return; }

        print("Win");
    }

    public override void ReceiveElement(Element element)
    {
        if (heldElements.Contains(element) || !requiredElements.Contains(element)) { print("Bad pass"); return; }

        heldElements.Add(element);
        CheckElements();
    }
}
