using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GraphicRaycaster))]
public class MouseOverUIChecker : MonoBehaviour
{
    private GraphicRaycaster ray;
    private EventSystem eventSys;

    private void Awake()
    {
        ray = GetComponent<GraphicRaycaster>();

        eventSys = FindObjectOfType<EventSystem>();
    }

    public bool GetIsOverUI(Vector3 inputPos)
    {

        PointerEventData pointData = new PointerEventData(eventSys);
        pointData.position = inputPos;

        List<RaycastResult> results = new List<RaycastResult>();

        ray.Raycast(pointData, results);

        if (results.Count != 0)
            return true;
        else
            return false;
    }
}
