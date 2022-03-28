using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour
{
    [SerializeField] List<Element> requiredElements = new List<Element>();
    private Dictionary<Element, bool> requiredElementsObtained = new Dictionary<Element, bool>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.parent.TryGetComponent(out PlayerElementManager player))
        {
            CheckElements(player.HeldElements);
            other.gameObject.SetActive(false);
        }
    }

    protected void UpdateSprite()
    {
        //Change sprite
    }

    private void CheckElements(Element[] elements)
    {
        if(elements.Length != requiredElements.Count) { return; }

        Dictionary<Element, bool> requiredElementsObtained = new Dictionary<Element, bool>();

        foreach (var element in requiredElements)
        {
            if (element == Element.None) continue;
            requiredElementsObtained.Add(element, false);
        }

        foreach (Element element in requiredElements)
        {
            if (elements.ToList<Element>().Contains(element))
            {
                requiredElementsObtained[element] = true;
            }
        }

        if (requiredElementsObtained.ContainsValue(false)) return;

        print("Win");
    }
}
