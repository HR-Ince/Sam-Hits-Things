using UnityEngine;
using UnityEngine.Events;

public class LaunchModule : MonoBehaviour
{
    [SerializeField] UnityEvent onJump;

    public void Launch(Rigidbody body, Vector3 launchDirection, float drawPercentage, float thrust, ForceMode forceMode)
    {
        Vector3 force = launchDirection * (drawPercentage * thrust);
        body.AddForce(force, forceMode);

        print("direction: " + launchDirection + ", drawPercentage: " + drawPercentage + "thrust: " + thrust);
    }
}
