using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] UnityEvent onClick;

    public delegate void OnClick();

    public void OnPointerDown(PointerEventData data)
    {
        if(onClick != null)
        {
            onClick.Invoke();
        }        
    }
}
