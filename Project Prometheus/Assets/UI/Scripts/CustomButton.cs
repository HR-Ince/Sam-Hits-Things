using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent onClick;
    [SerializeField] ActiveObjects actives;

    private void OnEnable()
    {
        if(actives.ActiveButton == null) { actives.SetActiveButton(gameObject); }
        else if(actives.ActiveButton != this)
        {
            actives.ActiveButton.SetActive(false);
            print("button off");
            actives.SetActiveButton(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (onClick != null)
            onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData data)
    {
        gameObject.SetActive(false);
    }
}
