using UnityEngine;

public abstract class ElementVesselController : MonoBehaviour
{
    

    protected SpriteRenderer _renderer;

    protected void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        UpdateSprite();
    }
    public abstract ElementVesselController ReportSelf();
    public abstract bool CanReceiveElement();
    public abstract bool CanGiveElement();
    protected abstract void UpdateSprite();
    public abstract void ReceiveElement(Element element);
}
