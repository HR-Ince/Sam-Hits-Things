using UnityEngine;

public class LaunchModule : MonoBehaviour
{
    public void Launch(Rigidbody body, PlayerDrawHandler targeter, float thrust, ForceMode forceMode)
    {
        Vector3 force = targeter.DirectionVector * (targeter.DrawPercentage * thrust);
        body.AddForce(force, forceMode);
    }
}
