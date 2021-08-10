using System.Collections.Generic;
using UnityEngine;

public enum Element { None, Wind, Earth, Fire, Water }

public class ChaplainElementHandler : MonoBehaviour
{
    [SerializeField] GameEvent onElementUpdate;
    [SerializeField] ActiveObjects actives;
    
    public List<Element> HeldElements { get { return heldElements; } }
    public List<Element> DisplayElements { get { return displayElements; } }

    [SerializeField] private List<Element> heldElements = new List<Element>();
    [SerializeField] private List<Element> displayElements = new List<Element>();

    private ChaplainPlayerController controller;

    private void Awake()
    {
        controller = GetComponent<ChaplainPlayerController>();
    }
    
    public void AddElement(Element element)
    {
        heldElements.Add(element);
        displayElements.Add(element);
        if(onElementUpdate != null) { onElementUpdate.Invoke(); }
    }

    public void RemoveElement(Element element)
    {
        heldElements.Remove(element);
    }

    private void RemoveDisplayElement(Element element)
    {
        displayElements.Remove(element);
        if (onElementUpdate != null) { onElementUpdate.Invoke(); }
    }

    public void InfuseDemon(Element element)
    {
        controller.Demons[0].GetComponent<DemonElementHandler>().HandleElement(element);
        RemoveDisplayElement(element);
    }

    public void InfuseShrine(Element element)
    {
        actives.ActiveShrine.ReceiveElement(element);
    }
}
