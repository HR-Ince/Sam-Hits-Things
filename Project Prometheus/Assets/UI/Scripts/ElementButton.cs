using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ElementButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<Element> OnButtonPress;
    
    private Element _associatedElement;

    // Component references
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();

        MarkEmpty();
    }

    public void SetAssociatedElementAndUpdate(Element element)
    {
        _associatedElement = element;
        _text.text = _associatedElement.ToString();
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;

        if(OnButtonPress != null) OnButtonPress.Invoke(_associatedElement);
    }

    public void MarkEmpty()
    {
        _text.text = "Empty";
    }
}
