using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerColliderToBrakePad : MonoBehaviour
{
    [SerializeField] float brakeDrag = 3f;

    private float defaultDrag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Rigidbody rB))
        {
            defaultDrag = rB.drag;
            rB.drag = brakeDrag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rB))
        {
            rB.drag = defaultDrag;
        }
    }
}
