using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] GameObject defaultObject;
    [SerializeField] GameObject[] conditionalObjects;
    [SerializeField] ActiveObjects actives;

    [SerializeField] private bool[] conditionsMet;

    private void Awake()
    {
        conditionsMet = new bool[conditionalObjects.Length];
        DeactivateMenu();
    }

    public void ActivateMenu()
    {
        HandleMenuActivation();
        defaultObject.SetActive(true);
        HandleConditionalObjects();
    }

    private void HandleMenuActivation()
    {
        if (actives.ActiveMenu == null) 
        { 
            actives.SetActiveMenu(this);
            return;
        }
        if (actives.ActiveMenu != this)
        {
            actives.ActiveMenu.DeactivateMenu();
            actives.SetActiveMenu(this);
        }
    }

    private void HandleConditionalObjects()
    {
        if(conditionalObjects.Length <= 0 || conditionsMet.Length <= 0) { return; }
        for (int i = 0; i < conditionalObjects.Length; i++)
        {
            
            if (conditionsMet[i])
                conditionalObjects[i].SetActive(true);
        }
    }

    public void SetConditionMet(int conditionNo, bool value)
    {
        conditionsMet[conditionNo] = value;
    }

    public void DeactivateMenu()
    {
        defaultObject.SetActive(false);
        foreach (GameObject obj in conditionalObjects)
        {
            obj.SetActive(false);
        }
    }
}
