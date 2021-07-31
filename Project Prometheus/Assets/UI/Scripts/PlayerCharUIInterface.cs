using UnityEngine;

public class PlayerCharUIInterface : BaseUIInterface
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 spriteOffset;
    [SerializeField] Vector3 scale;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        menu = contextMenu;
        transform.position = cam.WorldToScreenPoint(player.transform.position + spriteOffset);
        transform.localScale = scale;
    }
}
