using UnityEngine;

public class DemonController : MonoBehaviour
{
    [SerializeField] ActiveObjects actives;
    [SerializeField] DemonUIInterface uiInterface;

    private AnchorController myAnchor;

    private Transform canvas;
    private Rigidbody rB;
    private DemonUIInterface myInterface;

    private void Awake()
    {
        FetchExternalVariables();
        SetupInterface();
    }
    private void FetchExternalVariables()
    {
        rB = GetComponent<Rigidbody>();
        canvas = FindObjectOfType<Canvas>().transform;
    }
    private void SetupInterface()
    {
        myInterface = Instantiate(uiInterface, canvas);
        myInterface.SetHost(gameObject);
        myInterface.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        actives.SetActiveDemon(gameObject);
        SetAnchorConditional();
    }
    private void OnMouseOver()
    {
        if(rB.velocity == Vector3.zero)
        {
            myInterface.Activate();
        }
    }

    public void SetNearbyAnchor(AnchorController anchor)
    {
        myAnchor = anchor;
    }

    private void SetAnchorConditional()
    {
        if (myAnchor != null && myAnchor.HoldingElement)
        {
            myInterface.SetConditional(0, true);
        }
        else if(myAnchor == null || !myAnchor.HoldingElement)
            myInterface.SetConditional(0, false);
    }

    public void ResetAnchorContact()
    {
        myInterface.SetConditional(0, false);
        myAnchor = null;
    }

    private void OnMouseExit()
    {
        myInterface.gameObject.SetActive(false);
    }

    public void SetContextMenu(GameObject menu)
    {
        myInterface.SetContextMenu(menu);
    }
}
