using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaplainUIInterface : BaseUIInterface, IPointerDownHandler
{
    [SerializeField] GameObject player;

    private ChaplainElementHandler elementHandler;

    private new void Awake()
    {
        base.Awake();
        FetchExternalVariables();
        transform.position = cam.WorldToScreenPoint(player.transform.position + spriteOffset);
        transform.localScale = scale;
    }
    private void FetchExternalVariables()
    {
        elementHandler = player.GetComponent<ChaplainElementHandler>();
        menu = contextMenu;
        menuManager = menu.GetComponent<ContextMenuManager>();
    }

    public new void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);

    }

    protected override void EvaluateConditionals()
    {

    }

    private void SetElementButtons()
    {
        List<string> elementStrings = new List<string>();
        foreach(Element element in elementHandler.HeldElements)
        {
            elementStrings.Add(element.ToString());
        }
    }
}
