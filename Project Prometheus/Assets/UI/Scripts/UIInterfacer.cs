using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInterfacer : MonoBehaviour
{
    [SerializeField] ContextMenuManager contextMenu;
    [SerializeField] Vector3 menuOffset;
    [SerializeField] UnityEvent onClick;
    [SerializeField] PlayerInput player;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        print(gameObject.name + " pressed.");
        contextMenu.transform.position = cam.WorldToScreenPoint(transform.position + menuOffset);
        contextMenu.ActivateMenu();
        if (onClick != null)
            onClick.Invoke();
        player.WasUIClicked = true;
    }

    private void OnMouseUp()
    {
        player.WasUIClicked = false;
        EventSystem.current.SetSelectedGameObject(contextMenu.gameObject);
    }
}
