using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementButton : MonoBehaviour
{
    [SerializeField] ElementMasterHandler master;

    const string ELEMENT_ENCOUNTER = "EButton_Element encounter";
    const string DISPLAY_EARTH_BOOL = "shouldDisplayEarth";
    const string DISPLAY_WATER_BOOL = "shouldDisplayWater";
    const string DISPLAY_WIND_BOOL = "shouldDisplayWind";
    const string DISPLAY_FIRE_BOOL = "shouldDisplayFire";
    const string DISPLAY_NONE_BOOL = "shouldDisplayNone";
    const string EARTH = "EButton_Earth";
    const string OUT_IDLE = " output idle";
    const string OUT = " output";
    const string IN_IDLE = " input idle";
    const string IN = " input";

    private string heldElementString;
    private string elementAnimBool;
    private Element heldElement;
    private Element anchorElement;

    private Animator anim;
    private Image image;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        heldElement = master.DemonElement;
    }

    public void UpdateButton()
    {
        anchorElement = master.AnchorElement;
        
        if (anchorElement == heldElement) return;
        else if(anchorElement != Element.None && heldElement == Element.None)
        {
            UpdateButtonNoElementHeld();
        }
        else if(master.hasActiveAnchor && anchorElement == Element.None && heldElement != Element.None)
        {
            UpdateButtonEmptyAnchor();
        }
    }

    private void UpdateButtonNoElementHeld()
    {
        SetAnimStringsByElement(anchorElement);
        anim.Play(ELEMENT_ENCOUNTER);
        anim.SetBool(elementAnimBool, true);
        
    }

    private void UpdateButtonEmptyAnchor()
    {
        SetAnimStringsByElement(heldElement);

        anim.Play(heldElementString + OUT_IDLE);
    }

    private void SetAnimStringsByElement(Element element)
    {
        if (element == Element.None) return;

        if (element == Element.Earth) 
        { 
            heldElementString = EARTH;
            elementAnimBool = DISPLAY_EARTH_BOOL;
        }
    }
    public void OnPressed()
    {
        if(heldElement == Element.None && anchorElement != Element.None)
        {
            SetAnimStringsByElement(anchorElement);

            anim.Play(heldElementString + IN);
            master.SetDemonElement(anchorElement);
            heldElement = anchorElement;
            master.SetActiveAnchorElement(Element.None);
        }
        else if(master.hasActiveAnchor && anchorElement == Element.None && heldElement != Element.None)
        {
            SetAnimStringsByElement(heldElement);

            anim.Play(heldElementString + OUT);
            master.SetActiveAnchorElement(heldElement);
            master.SetDemonElement(Element.None);
            heldElement = Element.None;
        }

        //UpdateButton();
    }
}
