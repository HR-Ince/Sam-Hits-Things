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
        if(controller.MyVessel != null && controller.MyVessel.ReportSelf() is ShrineController)
            actives.SetActiveShrine(controller.MyVessel as ShrineController);
    }

    protected override void EvaluateConditionals()
    {
        SetVesselConditionals(controller.MyVessel);
        SetAbilityConditional();
    }

    private void SetVesselConditionals(ElementVesselController vessel)
    {
        if(vessel == null)
        {
            menuManager.SetConditional("Button_ElementIn", false);
            menuManager.SetConditional("Button_ElementOut", false); 
            return;
        }

        menuManager.SetConditional("Button_ElementIn", vessel.CanGiveElement());
        menuManager.SetConditional("Button_ElementOut", vessel.CanReceiveElement());
    }

    private void SetAbilityConditional()
    {
        menuManager.SetConditional("Button_ElementalDoings", elementHandler.IsHoldingElement);
    }
}
