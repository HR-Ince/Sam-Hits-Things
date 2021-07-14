using UnityEngine;

public class SummonFXAnimHelper : MonoBehaviour
{
    [SerializeField] GameEvent OnRecall;
    [SerializeField] SpriteRenderer demonSprite;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, transform.parent.rotation.z * -1);
    }

    public void ToggleSpriteOff()
    {
        demonSprite.enabled = false;
    }

    public void ToggleSpriteOn()
    {
        demonSprite.enabled = true;
    }

    public void Recall()
    {
        if (OnRecall != null)
            OnRecall.Invoke();
    }
}
