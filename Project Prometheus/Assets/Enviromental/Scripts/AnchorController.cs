using UnityEngine;

public class AnchorController : MonoBehaviour
{
    // Private variables
    [SerializeField] private Element _heldElement;
    [SerializeField] private Sprite _defaultSprite, _activeSprite;

    private bool _isActive = false;

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
        if (_isActive) return;

        if (other.gameObject.transform.parent.TryGetComponent(out PlayerElementManager player))
        {
            player.AddUsableElementAndUpdate(_heldElement);
            UpdateSprite();
            other.gameObject.SetActive(false);
            _isActive = true;
        }
        else { print("No manager"); }
    }

    private void UpdateSprite()
    {
        _renderer.sprite = _activeSprite;
    }
}
