using UnityEngine;
using UnityEngine.Events;

public class SphereDetection : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] UnityEvent onSphereDetection;
    [SerializeField] UnityEvent onSphereLeaving;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == sphere)
        {
            if (onSphereDetection != null)
                onSphereDetection.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == sphere)
            if (onSphereLeaving != null)
                onSphereLeaving.Invoke();
    }
}
