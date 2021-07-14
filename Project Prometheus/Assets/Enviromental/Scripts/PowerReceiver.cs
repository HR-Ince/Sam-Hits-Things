using UnityEngine;

public class PowerReceiver : MonoBehaviour
{
    [SerializeField] int interactableLayerNo;
    [SerializeField] GameEvent onPowerOn;

    private bool isPowered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == interactableLayerNo && !isPowered)
        {
            print("On");
            isPowered = true;
            other.transform.position = transform.position;
            if (other.TryGetComponent(out Rigidbody rigidbody))
                rigidbody.isKinematic = true;
            if (onPowerOn != null)
                onPowerOn.Invoke();
        }
    }
}
