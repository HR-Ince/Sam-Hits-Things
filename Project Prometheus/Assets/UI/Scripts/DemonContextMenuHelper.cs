using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContextMenuManager))]
public class DemonContextMenuHelper : MonoBehaviour
{
    [SerializeField] string[] conditionalStrings;
    [SerializeField] ActiveObjects actives;

    private ChaplainElementHandler chaplain;

    private void Awake()
    {
        chaplain = FindObjectOfType<ChaplainElementHandler>();
    }

    public void ElementSwap()
    {
        DemonController controller = actives.ActiveDemon.GetComponent<DemonController>();
        DemonElementHandler demon = actives.ActiveDemon.GetComponent<DemonElementHandler>();
        

        Element cache = demon.HeldElement;
        demon.Infuse(controller.MyAnchor.HeldElement);
        if(controller.MyAnchor.HeldElement == Element.None) { chaplain.RemoveElement(controller.MyAnchor.HeldElement); }
        else { chaplain.AddElement(controller.MyAnchor.HeldElement); }
        controller.MyAnchor.Infuse(cache);
    }
}
