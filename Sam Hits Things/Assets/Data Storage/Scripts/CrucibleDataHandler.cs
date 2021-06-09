using UnityEngine;

public enum Element { None, Wind, Earth, Fire, Water }

[CreateAssetMenu]
public class CrucibleDataHandler : ScriptableObject
{
    [SerializeField] GameEvent onTriggered;
    
    public CrucibleElementHandler Crucible { get { return crucible; } }
    public Element CrucibleElement { get { return crucibleElement; } }

    private CrucibleElementHandler crucible;
    private Element crucibleElement;

    private void OnEnable()
    {
        ClearActiveCrucible();
    }

    public void SetElementFrom(CrucibleElementHandler handler, Element element)
    {
        crucible = handler;
        crucibleElement = handler.HeldElement;
        if (onTriggered != null)
            onTriggered.Invoke();
    }

    public void SetActiveCrucibleElement(Element element)
    {
        crucibleElement = element;
        crucible.SetElement(crucibleElement);
        if (onTriggered != null)
            onTriggered.Invoke();
    }

    public void ClearActiveCrucible()
    {
        crucible = null;
        crucibleElement = Element.None;
        if (onTriggered != null)
            onTriggered.Invoke();
    }
}
