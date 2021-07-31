using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data Storage/Active Demon Holder")]
public class ActiveObjects : ScriptableObject
{
    public Element ActiveElement { get { return activeElement; } }
    public void SetActiveElement(Element element) { activeElement = element; }
    private Element activeElement;

    public GameObject ActiveDemon { get { return activeDemon; } }
    public void SetActiveDemon(GameObject demon) { activeDemon = demon; }

    [SerializeField] private GameObject activeDemon;

    public ContextMenuManager ActiveMenu { get { return activeMenu; } }
    public void SetActiveMenu(ContextMenuManager menu) { activeMenu = menu; }

    [SerializeField] private ContextMenuManager activeMenu;

    public AnchorController[] ActiveAnchors { get { return activeAnchors.ToArray(); } }
    public void AddActiveAnchor(AnchorController anchor) { activeAnchors.Add(anchor); }
    public void RemoveActiveAnchor(AnchorController anchor) { activeAnchors.Remove(anchor); }
    private List<AnchorController> activeAnchors;

    private void OnEnable()
    {
        activeElement = Element.None;
        activeDemon = null;
        activeMenu = null;
        activeAnchors.Clear();
    }
}
