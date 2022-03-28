using UnityEngine;

public class AnchorController : MonoBehaviour
{
    [SerializeField] Element _heldElement;
    [SerializeField] protected Sprite _defaultSprite, _activeSprite;

    // Component References
    private SpriteRenderer _renderer;

    private void Awake()
    {
        GetComponentReferences();
    }

    private void GetComponentReferences()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.TryGetComponent(out PlayerElementManager player))
        {
            player.AddUsableElementAndUpdate(_heldElement);
            UpdateSprite();
            other.gameObject.SetActive(false);
        }
        else { print("No manager"); }
    }

    private void UpdateSprite()
    {
        _renderer.sprite = _activeSprite;

    }
}
