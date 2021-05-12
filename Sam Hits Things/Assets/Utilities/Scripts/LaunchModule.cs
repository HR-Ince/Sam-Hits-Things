using UnityEngine;
using UnityEngine.Events;

public class LaunchModule : MonoBehaviour
{
    [SerializeField] float thrustModifier;
    [SerializeField] UnityEvent onJump;

    public float ThrustModifier { get { return thrustModifier; } }

    public void Launch(Rigidbody body, Vector3 launchDirection, float drawPercentage)
    {
        Launch(body, launchDirection, drawPercentage, 0);
    }

    public void Launch(Rigidbody body, Vector3 launchDirection, float drawPercentage, float additionalThrust)
    {
        float thrust = drawPercentage * (thrustModifier + additionalThrust);
        body.AddForce(launchDirection * thrust);
        if (onJump != null)
        {
            onJump.Invoke();
        }
    }
}
