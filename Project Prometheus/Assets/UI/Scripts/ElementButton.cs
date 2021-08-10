using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ElementButton : CustomButton, IPointerDownHandler
{
    [SerializeField] Element myElement;
    [SerializeField] UnityEvent<Element> onElementPress;

    public new void OnPointerDown(PointerEventData data)
    {
        if(onElementPress != null) { onElementPress.Invoke(myElement); }
    }
}
