using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Element { None, Wind, Earth, Fire, Water }

public class PlayerElementManager : MonoBehaviour
{
    // Public variables
    public Element[] HeldElements { get { return UsableElements.ToArray(); } }

    private List<Element> UsableElements = new List<Element>();

    // Private variables
    [SerializeField] private TMP_Text[] ElementTexts;

    private Element activeElement;
    private GameObject _nextUsedVessel;

    private delegate void ActivateAbility();
    private ActivateAbility activateAbility;

    private void Awake()
    {
        activeElement = Element.None;
        UpdateUI();
    }

    // Injections
    public void SetNextUsedVessel(GameObject vessel)
    {
        _nextUsedVessel = vessel;
    }
    private void SetActiveElement(Element elementToActivate)
    {
        activeElement = elementToActivate;
    }
    private void AddUsableElement(Element elementToAdd)
    {
        if (UsableElements.Contains(elementToAdd)) return;

        UsableElements.Add(elementToAdd);
    }

    public void SetActiveElementAndUpdate(Element elementToActivate)
    {
        SetActiveElement(elementToActivate);
        UpdateAbility(elementToActivate);
    }

    public void AddUsableElementAndUpdate(Element elementToAdd)
    {
        AddUsableElement(elementToAdd);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(UsableElements.Count <= 0)
        {
            foreach(TMP_Text textElement in ElementTexts)
            {
                textElement.text = "Empty";
            }
            return;
        }

        for (int i = 0; i < UsableElements.Count; i++)
        {
            ElementTexts[i].text = UsableElements[i].ToString();
        }
    }

    private void UpdateAbility(Element abilityElement)
    {
        if(abilityElement == Element.None)
        {
            activateAbility = ElementAbilityNone;
            return;
        }
        else if (abilityElement == Element.Earth)
        {
            activateAbility = ElementAbilityEarth;
            return;
        }
        else if (abilityElement == Element.Fire)
        {
            activateAbility = ElementAbilityFire;
            return;
        }
        else if (abilityElement == Element.Wind)
        {
            activateAbility = ElementAbilityWind;
            return;
        }
        else if (abilityElement == Element.Water)
        {
            activateAbility = ElementAbilityWater;
            return;
        }
    }

    public void OnAbilityActivated()
    {
        activateAbility();
    }

    private void ElementAbilityNone()
    {
        print("No ability");
    }
    private void ElementAbilityEarth()
    {
        print("Earth ability");
    }
    private void ElementAbilityWind()
    {
        print("Wind ability");
    }
    private void ElementAbilityFire()
    {
        print("Fire ability");
    }
    private void ElementAbilityWater()
    {
        print("Water ability");
    }
}
