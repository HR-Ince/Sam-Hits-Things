using UnityEngine;

public class DemonContextMenuFunctions : MonoBehaviour
{
    [SerializeField] ActiveObjects actives;

    private ChaplainElementHandler chaplain;
    private ChaplainUIInterface chaplainUI;

    private void Awake()
    {
        chaplain = FindObjectOfType<ChaplainElementHandler>();
        chaplainUI = FindObjectOfType<ChaplainUIInterface>();
    }

    private void FetchDemonVariables(out ElementVesselController vessel, out DemonElementHandler demon)
    {
        vessel = actives.ActiveDemon.GetComponent<DemonController>().MyVessel;
        demon = actives.ActiveDemon.GetComponent<DemonElementHandler>();
    }

    public void ElementIn()
    {
        FetchDemonVariables(out ElementVesselController vessel, out DemonElementHandler demon);

        if (vessel.ReportSelf() is AnchorController anchor)
        {
            chaplain.AddElement(anchor.HeldElement);
            anchor.ReceiveElement(Element.None);
        }
    }

    public void ElementOut()
    {
        FetchDemonVariables(out ElementVesselController vessel, out DemonElementHandler demon);

        if(vessel.ReportSelf() is AnchorController anchor)
        {
            anchor.ReceiveElement(demon.HeldElement);
            chaplain.RemoveElement(demon.HeldElement);
            demon.HandleElement(Element.None);
        }
        else if(vessel.ReportSelf() is ShrineController)
        {
            chaplainUI.CreateShrineMenu();
        }
        
    }

    public void ElementalAbility()
    {
        FetchDemonVariables(out ElementVesselController vessel, out DemonElementHandler demon);
        print(demon.gameObject.name + " does a " + demon.HeldElement.ToString() + " thing.");
    }

}
