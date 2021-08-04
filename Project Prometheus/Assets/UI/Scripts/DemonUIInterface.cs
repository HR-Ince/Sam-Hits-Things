using UnityEngine;
using UnityEngine.EventSystems;

public class DemonUIInterface : BaseUIInterface, IPointerDownHandler
{
    [SerializeField] ActiveObjects actives;

    private GameObject host;
    private DemonController controller;
    private DemonElementHandler elementHandler;

    public void SetHost(GameObject value)
    {
        host = value;
        elementHandler = host.GetComponent<DemonElementHandler>();
        controller = host.GetComponent<DemonController>();
    }
    public void SetContextMenu(GameObject obj)
    {
        menu = obj;
        menuManager = menu.GetComponent<ContextMenuManager>();
    }

    private void Update()
    {
        transform.position = cam.WorldToScreenPoint(host.transform.position + spriteOffset);
    }

    public new void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data);
        actives.SetActiveDemon(host);
    }

    protected override void EvaluateConditionals()
    {
        SetAnchorConditionals(controller.MyAnchor);
    }

    private void SetAnchorConditionals(AnchorController anchor)
    {
        if(anchor == null)
        {
            menuManager.SetConditional("Button_ElementIn", false);
            menuManager.SetConditional("Button_ElementOut", false); 
            return;
        }

        menuManager.SetConditional("Button_ElementIn", anchor.IsHoldingElement);
        menuManager.SetConditional("Button_ElementOut", !anchor.IsHoldingElement && elementHandler.IsHoldingElement);
    }
}
