using UnityEngine;

public class CrucibleElementHandler : MonoBehaviour
{
    [SerializeField] Element heldElement;
    [SerializeField] ElementMasterHandler data;
    [SerializeField] Sprite PH_None, PH_earth;

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
    }

    public void SetElement(Element element)
    {
        heldElement = element;
        UpdateSprite();
    }

    public void onSphereLeaving()
    {
        data.ClearActiveCrucible();
    }
}
