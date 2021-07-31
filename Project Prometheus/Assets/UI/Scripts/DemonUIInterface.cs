using UnityEngine;

public class DemonUIInterface : BaseUIInterface
{
    [SerializeField] Vector3 spriteOffset;
    [SerializeField] Vector3 scale;

    private Camera cam;
    private ContextMenuManager menuManager;
    private GameObject host;

    public void SetHost(GameObject value)
    {
        host = value;
    }
    public void SetContextMenu(GameObject obj)
    {
        menu = obj;
        menuManager = menu.GetComponent<ContextMenuManager>();
    }

    private void Awake()
    {
        cam = Camera.main;
        transform.localScale = scale;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        transform.position = cam.WorldToScreenPoint(host.transform.position + spriteOffset);
    }

    public void SetConditional(int conditionalNo, bool value)
    {
        menuManager.SetConditionMet(conditionalNo, value);
    }
}
