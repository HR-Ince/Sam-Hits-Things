using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseUIInterface : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected GameObject contextMenu;
    [SerializeField] protected Vector3 spriteOffset;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] protected Vector3 scale;

    protected Camera cam;
    protected ContextMenuManager menuManager;
    protected GameObject menu;

    protected void Awake()
    {
        cam = Camera.main;
        transform.localScale = scale;
    }

    public void OnPointerDown(PointerEventData data)
    {
        EvaluateConditionals();
        CreateMenu();
    }
    protected void CreateMenu()
    {
        Vector3 offset;
        if (transform.position.x + contextMenu.GetComponent<RectTransform>().rect.width > Screen.width)
        {
            offset = new Vector3(-menuOffset.x, menuOffset.y, menuOffset.z);
        }
        else
            offset = menuOffset;

        menu.transform.position = transform.position + offset;
        menu.GetComponent<ContextMenuManager>().ActivateMenu();
    }
    protected abstract void EvaluateConditionals();
}
