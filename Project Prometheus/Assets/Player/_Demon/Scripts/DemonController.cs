using UnityEngine;

public class DemonController : MonoBehaviour
{
    [SerializeField] DemonUIInterface uiInterface;

    public AnchorController MyAnchor { get { return myAnchor; } }

    private Transform canvas;
    private DemonUIInterface myInterface;

    private AnchorController myAnchor;
    
    private void Awake()
    {
        FetchExternalVariables();
        SetupInterface();
    }
    private void FetchExternalVariables()
    {
        canvas = FindObjectOfType<Canvas>().transform;
    }
    private void SetupInterface()
    {
        myInterface = Instantiate(uiInterface, canvas);
        myInterface.SetHost(gameObject);
    }

    public void SetNearbyAnchor(AnchorController anchor)
    {
        myAnchor = anchor;
    }
    public void ResetAnchorContact()
    {
        myAnchor = null;
    }

    public void SetContextMenu(GameObject menu)
    {
        myInterface.SetContextMenu(menu);
    }
}
