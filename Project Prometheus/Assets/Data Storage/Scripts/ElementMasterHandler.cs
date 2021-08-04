using UnityEngine;

[CreateAssetMenu]
public class ElementMasterHandler : ScriptableObject
{
    [SerializeField] GameEvent onTriggered;
    
    public bool HasActiveAnchor { get { return anchor != null; } }
    public Element AnchorElement { get { return anchorElement; } }
    public Element DemonElement { get { return demonElement; } }

    private DemonElementHandler demon;
    private CrucibleElementHandler anchor;
    private Element anchorElement;
    private Element demonElement;

    private void OnEnable()
    {
        ClearActiveCrucible();
        demonElement = Element.None;
    }

    public void SetDemon(DemonElementHandler handler)
    {
        demon = handler;
    }

    public void SetElementFrom(CrucibleElementHandler handler, Element element)
    {
        anchor = handler;
        anchorElement = element;
        if (onTriggered != null)
            onTriggered.Invoke();
    }

    public void SetActiveAnchorElement(Element element)
    {
        anchor.SetElement(element);
        anchorElement = element;
    }

    public void SetDemonElement(Element element)
    {
        demon.Infuse(element);
        demonElement = element;
    }

    public void ClearActiveCrucible()
    {
        anchor = null;
        anchorElement = Element.None;
        if (onTriggered != null)
            onTriggered.Invoke();
    }
}
