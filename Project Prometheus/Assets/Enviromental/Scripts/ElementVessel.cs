using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ElementVessel : MonoBehaviour
{
    [SerializeField] Element[] heldElements;
    [SerializeField] Sprite noneSprite, earthSprite;

    public bool IsHoldingElement { get { return heldElements.Length > 0; } }
    public Element HeldElement { get { return heldElement; } }
    private Element heldElement;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        heldElement = heldElements[0];
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (heldElement == Element.None) { _renderer.sprite = noneSprite; }
        if (heldElement == Element.Earth) { _renderer.sprite = earthSprite; }
    }
    
    public void HandleElement(Element element)
    {
        heldElement = element;
        UpdateSprite();
    }
}
