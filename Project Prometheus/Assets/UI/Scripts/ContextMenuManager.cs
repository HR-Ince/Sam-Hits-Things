using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] GameObject defaultObject;
    [SerializeField] ActiveObjects actives;
    [SerializeField] GameObject sampleButton;

    public Dictionary<GameObject, bool> conditions = new Dictionary<GameObject, bool>();
    private List<GameObject> conditionalObjects = new List<GameObject>();

    private PlayerInput input;

    private void Awake()
    {
        OrganiseCoditionals();
        input = FindObjectOfType<PlayerInput>();
        DeactivateMenu();
    }
    private void OrganiseCoditionals()
    {
        var temp = GetComponentsInChildren<CustomButton>();
        for(int i = 0; i < temp.Length; i++)
        {
            if(temp[i].gameObject == defaultObject) { continue; }
            conditions.Add(temp[i].gameObject, false);
            conditionalObjects.Add(temp[i].gameObject);
        }
    }

    public void ActivateMenu()
    {
        HandleActiveMenu();
        if (defaultObject != null) { defaultObject.SetActive(true); }
        SetActiveConditionals();
    }
    private void HandleActiveMenu()
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
    public void SetConditional(string name, bool value)
    {
        foreach(GameObject obj in conditions.Keys)
        {
            if(obj.name == name) 
            { 
                conditions[obj] = value;
                return;
            }
        }

        print("Context Menu Manager could not find an object with name " + name);
    }
    private void SetActiveConditionals()
    {
        foreach(KeyValuePair<GameObject, bool> pair in conditions)
        {
            pair.Key.SetActive(pair.Value);
        }
    }

    private void Update()
    {
        if (input.GamePressed)
            DeactivateMenu();
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
