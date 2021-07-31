using UnityEngine;

public class CrucibleElementHandler : MonoBehaviour
{
    [SerializeField] Element heldElement;
    [SerializeField] ElementMasterHandler data;
    [SerializeField] Sprite PH_None, PH_earth;
    [SerializeField] ActiveObjects actives;

    public bool HasElement { get { return heldElement != Element.None; } }

    private GameObject demonHere;

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
    public void OnSphereDetection()
    {
        data.SetElementFrom(this, heldElement);
        //actives.SetActiveAnchor(this);
    }

    public void SetElement(Element element)
    {
        heldElement = element;
        UpdateSprite();
    }

    public void onSphereLeaving()
    {
        data.ClearActiveCrucible();
        //actives.SetActiveAnchor(null);
    }
}
