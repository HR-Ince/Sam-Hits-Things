using UnityEngine;

public class DemonController : MonoBehaviour
{
    [SerializeField] ActiveObjects holder;

    public void OnClick()
    {
        holder.SetActiveDemon(gameObject);
    }
}
