using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GraphicRaycaster))]
public class MouseOverUIChecker : MonoBehaviour
{
    public bool IsOverUI { get { return isOverUI; } }

    private bool isOverUI;

    private GraphicRaycaster ray;
    private EventSystem eventSys;

    private void Awake()
    {
        ray = GetComponent<GraphicRaycaster>();

        eventSys = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        PointerEventData pointData = new PointerEventData(eventSys);
        pointData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        ray.Raycast(pointData, results);

        if (results.Count != 0)
            isOverUI = true;
        else
            isOverUI = false;
    }
}
