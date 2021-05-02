using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerGrabHandler : MonoBehaviour
{
    [SerializeField] float reach;
    [SerializeField] Transform holdPos;
    [SerializeField] int interactableLayerNumber;
    [SerializeField] TMP_Text interactionText;
    [SerializeField] Vector3 interactionTextOffset;

    private Camera cam;
    private Collider[] overlappingColliders;
    private int layerMask;

    private PlayerInput input;
    private PlayerStateManager state;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        state = GetComponent<PlayerStateManager>();
        if(state == null) { Debug.LogError("State manager missing from player"); }

        cam = Camera.main;
        layerMask = 1 << interactableLayerNumber;
    }
    private void Update()
    {
        overlappingColliders = Physics.OverlapSphere(transform.position, reach, layerMask);

        if(state.IsStopped && overlappingColliders.Length > 0 && ! state.IsBurdened)
        {
            foreach(Collider obj in overlappingColliders)
            {
                interactionText.enabled = true;
                interactionText.text = "Grab";
                interactionText.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position + interactionTextOffset);

                if (input.Pressed)
                {
                    Grab(obj.transform.gameObject);
                }
            }
        }
        /*
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f))
        {
            if (hit.transform.gameObject.CompareTag("Holdable"))
            {
                if (!state.IsBurdened)
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);

                    if (dist < reach)
                    {
                        interactionText.enabled = true;
                        interactionText.text = "Grab";
                        interactionText.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position + interactionTextOffset);
                        if (input.Pressed)
                        {
                            Grab(hit.transform.gameObject);
                        }
                    }
                }
                else
                {
                    interactionText.enabled = true;
                    interactionText.text = "Release";
                    interactionText.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position + interactionTextOffset);
                    if (input.Pressed)
                        Release(hit.transform.gameObject);
                }
            }
            else
            {
                interactionText.enabled = false;
            }
        }*/
    }
    private void Grab(GameObject obj)
    {
        obj.transform.parent = transform;
        Rigidbody objRB = obj.GetComponent<Rigidbody>();
        objRB.isKinematic = true;
        obj.transform.position = holdPos.position;
        state.SetIsBurdened(true);
    }
    private void Release(GameObject obj)
    {
        obj.transform.parent = null;
        Rigidbody objRB = obj.GetComponent<Rigidbody>();
        objRB.isKinematic = false;
        objRB.AddForce(Vector3.left * 300);
        state.SetIsBurdened(false);
    }
}
