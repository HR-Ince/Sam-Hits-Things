using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UIInterfacer : MonoBehaviour
{
    [SerializeField] GameObject uiObject;
    [SerializeField] Vector3 uiOffset;
    [SerializeField] UnityEvent onClick;
    [SerializeField] PlayerInput player;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        uiObject.transform.position = cam.WorldToScreenPoint(transform.position + uiOffset);
        uiObject.SetActive(true);
        print("button on");
        if (onClick != null)
            onClick.Invoke();
        player.WasUIClicked = true;
    }

    private void OnMouseUp()
    {
        player.WasUIClicked = false;
    }
}
