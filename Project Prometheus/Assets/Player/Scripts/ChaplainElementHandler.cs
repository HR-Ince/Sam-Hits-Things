using System.Collections.Generic;
using UnityEngine;

public enum Element { None, Wind, Earth, Fire, Water }

public class ChaplainElementHandler : MonoBehaviour
{
    [SerializeField] GameEvent onElementUpdate;

    public Element[] HeldElements { get { return heldElements.ToArray(); } }

    private List<Element> heldElements = new List<Element>();
    
    public void AddElement(Element element)
    {
        heldElements.Add(element);
        if(onElementUpdate != null) { onElementUpdate.Invoke(); }
    }

    public void RemoveElement(Element element)
    {
        heldElements.Remove(element);
        if (onElementUpdate != null) { onElementUpdate.Invoke(); }
    }
}
