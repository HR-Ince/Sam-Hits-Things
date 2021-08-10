using UnityEngine;

public class DemonController : MonoBehaviour
{
    [SerializeField] DemonUIInterface uiInterface;

    public ElementVesselController MyVessel { get { return myVessel; } }

    private Transform canvas;
    private DemonElementHandler elementHandler;
    private DemonUIInterface myInterface;

    private ElementVesselController myVessel;
    private Buttonv2 myButton;

    public void SetContextMenu(GameObject menu)
    {
        myInterface.SetContextMenu(menu);
    }

    private void Awake()
    {
        FetchExternalVariables();
        SetupInterface();
    }
    private void FetchExternalVariables()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        elementHandler = GetComponent<DemonElementHandler>();
        if(elementHandler == null) { print("Fucked it"); }
    }
    private void SetupInterface()
    {
        myInterface = Instantiate(uiInterface, canvas);
        myInterface.SetHost(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Buttonv2 button))
        { myButton = button; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Buttonv2 button))
        { myButton = null; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ElementVesselController vessel))
        { 
            myVessel = vessel; 
            if(myVessel is AnchorController anchor)
                anchor.SetDemon(elementHandler);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ElementVesselController vessel))
        { myVessel = null; }
    }

    public void ResetAssociations()
    {
        if (myButton != null) { RemoveFromButton(); }
        ResetVesselContact();
    }
    private void ResetVesselContact()
    {
        myVessel = null;
    }
    private void RemoveFromButton()
    {
        myButton.OnTransformLeave(elementHandler);
        myButton = null;
    }
}
