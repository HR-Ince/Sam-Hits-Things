using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementButton : MonoBehaviour
{
    [SerializeField] Sprite emptyImage, earthImage, fireImage, waterImage, windImage;
    [SerializeField] Image secondaryImage;
    [SerializeField] CrucibleDataHandler data;
    [SerializeField] SphereElementHandler sphere;

    private Image image;
    private TMP_Text text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }
    private void Start()
    {
        UpdateButton();
    }

    public void UpdateButton()
    {
        UpdateText();
        UpdateImages();
    }

    private void UpdateText()
    {
        text.text = data.CrucibleElement.ToString();
    }

    private void UpdateImages()
    {
        if (sphere.HeldElement == Element.None)
            image.sprite = emptyImage;
        else if (sphere.HeldElement == Element.Earth)
            image.sprite = earthImage;
        else if (sphere.HeldElement == Element.Fire)
            image.sprite = fireImage;
        else if (sphere.HeldElement == Element.Water)
            image.sprite = waterImage;
        else if (sphere.HeldElement == Element.Wind)
            image.sprite = windImage;

        if (data.Crucible == null)
        {
            secondaryImage.enabled = false;
            return;
        }
        else
            secondaryImage.enabled = true;

        if (data.CrucibleElement == Element.None)
            secondaryImage.sprite = emptyImage;
        else if (data.CrucibleElement == Element.Earth)
            secondaryImage.sprite = earthImage;
        else if (data.CrucibleElement == Element.Fire)
            secondaryImage.sprite = fireImage;
        else if (data.CrucibleElement == Element.Water)
            secondaryImage.sprite = waterImage;
        else if (data.CrucibleElement == Element.Wind)
            secondaryImage.sprite = windImage;
    }

    public void OnPressed()
    {
        var cache = sphere.HeldElement;
        sphere.Infuse(data.CrucibleElement);
        data.SetActiveCrucibleElement(cache);
    }
}
