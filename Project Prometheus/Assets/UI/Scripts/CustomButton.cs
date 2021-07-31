using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] UnityEvent onClick;

    public void OnPointerDown(PointerEventData data)
    {
        if(onClick != null)
        {
            onClick.Invoke();
        }

        gameObject.SetActive(false);
    }
}
