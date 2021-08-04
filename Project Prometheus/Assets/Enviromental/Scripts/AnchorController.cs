using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AnchorController : MonoBehaviour
{
    [SerializeField] Element heldElement;
    [SerializeField] Sprite PH_None, PH_earth;

    public bool IsHoldingElement { get { return heldElement != Element.None; } }
    public Element HeldElement { get { return heldElement; } }

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (heldElement == Element.None) { _renderer.sprite = PH_None; }
        if (heldElement == Element.Earth) { _renderer.sprite = PH_earth; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out DemonController demonController))
        {
            demonController.SetNearbyAnchor(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DemonController demonController))
        {
            demonController.ResetAnchorContact();
        }
    }

    public void Infuse(Element element)
    {
        heldElement = element;
        UpdateSprite();
    }
}
