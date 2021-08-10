using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaplainUIInterface : BaseUIInterface, IPointerDownHandler
{
    [SerializeField] GameObject player;

    private ChaplainElementHandler elementHandler;

    private new void Awake()
    {
        base.Awake();
        FetchExternalVariables();
        transform.position = cam.WorldToScreenPoint(player.transform.position + spriteOffset);
        transform.localScale = scale;
    }
    private void FetchExternalVariables()
    {
        elementHandler = player.GetComponent<ChaplainElementHandler>();
        menu = contextMenu;
        menuManager = menu.GetComponent<ContextMenuManager>();
    }
    public void CreateShrineMenu()
    {
        SetShrineButtons();
        CreateMenu();
    }
    protected override void EvaluateConditionals()
    {
        SetElementButtons();
    }

    private void SetElementButtons()
    {
        SetButtons("Button_Shrine", new List<Element>());
        if(player.GetComponent<ChaplainPlayerController>().Demons.Length <= 0)
        {
            SetButtons("Button_", new List<Element>());
            return;
        }
        string conditionalString = "Button_";

        SetButtons(conditionalString, elementHandler.DisplayElements);   
    }
    
    private void SetShrineButtons()
    {
        string conditionalString = "Button_Shrine";

        SetButtons(conditionalString, elementHandler.HeldElements);
    }
    private void SetButtons(String conditionalString, List<Element> elementList)
    {
        foreach (Element element in (Element[])Enum.GetValues(typeof(Element)))
        {
            if (element == Element.None) { continue; }
            if (elementList.Contains(element))
                menuManager.SetConditional(conditionalString + element.ToString(), true);
            else
                menuManager.SetConditional(conditionalString + element.ToString(), false);
        }
    }
}
