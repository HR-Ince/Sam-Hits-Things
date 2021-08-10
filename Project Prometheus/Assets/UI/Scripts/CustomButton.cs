using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent onClick;
    [SerializeField] UnityEvent onButtonUp;
    public delegate void OnClick();

    public void OnPointerDown(PointerEventData data)
    {
        if(onClick != null)
        {
            onClick.Invoke();
        }        
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (onButtonUp != null)
            onButtonUp.Invoke();
    }
}
