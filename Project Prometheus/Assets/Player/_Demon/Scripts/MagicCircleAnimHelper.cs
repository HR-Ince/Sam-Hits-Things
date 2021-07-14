using UnityEngine;

public class MagicCircleAnimHelper : MonoBehaviour
{
    private PlayerAnimationController anim;

    private void Awake()
    {
        anim = GetComponentInParent<PlayerAnimationController>();
    }

    public void OnCircleOnAnimComplete()
    {
        anim.OnCircleOnAnimComplete();
    }
}
