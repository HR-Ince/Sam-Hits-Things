using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerElementTracker : MonoBehaviour
{
    [SerializeField] ChaplainElementHandler elementHandler;
    [SerializeField] Sprite defaultSprite, earthSprite;

    private Image[] icons;

    private int nextIndex;

    private void Awake()
    {
        EnableIconsFromAnchors();
    }
    private void EnableIconsFromAnchors()
    {
        AnchorController[] anchors = FindObjectsOfType<AnchorController>();
        List<Element> elementsInLevel = new List<Element>();

        foreach (AnchorController anchor in anchors)
        {
            if(anchor.HeldElement == Element.None) { continue; }
            if (!elementsInLevel.Contains(anchor.HeldElement))
            {
                elementsInLevel.Add(anchor.HeldElement);
            }
        }

        icons = GetComponentsInChildren<Image>();

        for (int i = 0; i < icons.Length; i++)
        {
            if (i < elementsInLevel.Count)
                icons[i].gameObject.SetActive(true);
            else
                icons[i].gameObject.SetActive(false);
        }
    }

    public void HandleElementChange()
    {
        if (elementHandler.DisplayElements.Count > nextIndex)
        {
            ActivateElementIcon();
        }
        else
            RemoveElement();
    }

    private void ActivateElementIcon()
    {
        Element newElement = elementHandler.DisplayElements[nextIndex];
        icons[nextIndex].sprite = ElementToSprite(newElement);
        nextIndex++;
    }
    private void RemoveElement()
    {
        nextIndex--;
        icons[nextIndex].sprite = defaultSprite;
    }

    private Sprite ElementToSprite(Element element)
    {
        if (element == Element.Earth) { return earthSprite; }
        else { return defaultSprite; }
    }
}
