using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlaySpaceManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent PointerDown;
    public UnityEvent PointerUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown != null) PointerDown.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(PointerUp != null) PointerUp.Invoke();
    }
}
