using UnityEngine;

public class ForegroundOcclusion : MonoBehaviour
{
    [SerializeField] PlayerStateRegister register;
    [SerializeField] WorldState world;

    private Transform player;
    private GameObject lastHitObject;
    private LayerMask mask;

    private void Awake()
    {
        player = register.PlayerOne.transform;
        mask = LayerMask.GetMask("Foreground");
    }
    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity, mask);

        if (hit.transform != null && lastHitObject == null)
        {
            hit.transform.gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            lastHitObject = hit.transform.gameObject;
        }
        else if(hit.transform == null && lastHitObject != null)
        {
            lastHitObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            lastHitObject = null;
        }
    }
}
