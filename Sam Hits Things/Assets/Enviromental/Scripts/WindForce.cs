using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WindForce : MonoBehaviour
{
    [SerializeField] float windSpeed;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(transform.up * windSpeed, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rigidbody))
        {
            float rand = Random.Range(-windSpeed, windSpeed);
            Vector3 exitForce = new Vector3(rand, windSpeed, 0);
            rigidbody.AddForce(exitForce, ForceMode.Force);
        }
    }
}
