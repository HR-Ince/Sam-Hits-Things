using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUIInterface : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected GameObject contextMenu;
    [SerializeField] Vector3 menuOffset;

    protected GameObject menu;

    public void OnPointerDown(PointerEventData data)
    {
        menu.transform.position = transform.position + menuOffset;
        menu.GetComponent<ContextMenuManager>().ActivateMenu();
    }
}
