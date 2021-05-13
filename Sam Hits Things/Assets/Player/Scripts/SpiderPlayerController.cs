using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class SpiderPlayerController : PlayerControllerBase, IPerishable
{
    private SphereCollider sphereCollider;
    private TargettingWithOrbit orbitTargetting;
 
    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;

        colliderHalfExtents = new Vector3(sphereCollider.radius, sphereCollider.radius, sphereCollider.radius);
        colliderCentre = sphereCollider.center;
    }
    private new void FetchExternalVariables()
    {
        base.FetchExternalVariables();
        orbitTargetting = targetting as TargettingWithOrbit;
        if(orbitTargetting == null) { Debug.LogError("Targetting manager missing from children of player"); }
        sphereCollider = _collider as SphereCollider;

    }

    private void Update()
    {
        GetIsGrounded();

        if (isGrounded)
        {
            if (input.Pressed)
            {
                orbitTargetting.SetupTargetting(transform.position, input.PressPos);
            }

            if (input.PressHeld)
            {
                orbitTargetting.AdjustTargetting(input.PressPos);
                if (orbitTargetting.DrawIsSufficient())
                {
                    line.ManageTrajectoryLine(launcher.ThrustModifier, 0f, _rigidbody.mass, ForceMode.Force);
                    line.enabled = true;
                }
                else
                    line.enabled = false;
            }
            if (input.PressReleased)
            {
                line.DisableLine();
                if (orbitTargetting.DrawIsSufficient())
                {
                    launcher.Launch(_rigidbody, orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage);
                }
            }
        }
        else
        {
            if (orbitTargetting.HasOrbitTarget())
            {
                if (input.Pressed)
                {
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.useGravity = false;

                    orbitTargetting.SetupTargetting(transform.position, input.PressPos);
                    orbitTargetting.GetOrbitCentre();
                }
                if (input.PressHeld)
                {
                    orbitTargetting.AdjustTargetting(input.PressPos);
                    if (orbitTargetting.DrawIsSufficient())
                    {
                        line.ManageTrajectoryLine(launcher.ThrustModifier, gravityMagnitude, _rigidbody.mass, ForceMode.Force);
                        line.enabled = true;
                    }
                }
                if (input.PressReleased)
                {
                    line.DisableLine();
                    transform.position = CalculateLaunchPosition(gravityMagnitude, ForceMode.Force);
                    _rigidbody.useGravity = true;
                    launcher.Launch(_rigidbody, orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, gravityMagnitude);
                    if (orbitTargetting.IsTargetMovable)
                    {
                        Rigidbody targetRB = orbitTargetting.Target.GetComponent<Rigidbody>();
                        if (targetRB.isKinematic)
                            targetRB.isKinematic = false;
                        launcher.Launch(targetRB, -orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, gravityMagnitude);
                    }

                    orbitTargetting.ClearTarget();
                }
            }
        }
    }
    private Vector3 CalculateLaunchPosition(float additionalThrust, ForceMode forceMode)
    {
        if(float.IsNaN(orbitTargetting.DirectionVector.x) || float.IsNaN(orbitTargetting.DirectionVector.y)) { return Vector3.zero; }

        float forceDuration = forceMode == ForceMode.Force || forceMode == ForceMode.Acceleration ? Time.fixedDeltaTime : 1;
        float objectMass = forceMode == ForceMode.Impulse || forceMode == ForceMode.Force ? _rigidbody.mass : 1;

        float thrust = 1 * (launcher.ThrustModifier + additionalThrust);
        Vector3 forceVector = orbitTargetting.DirectionVector * thrust;
        Vector3 velocityVector = (forceVector / objectMass) * forceDuration;

        float xPos = orbitTargetting.TargettingOrigin.x + velocityVector.x;
        float yPos = orbitTargetting.TargettingOrigin.y + velocityVector.y - 0.5f * gravityMagnitude;

        return new Vector3(xPos, yPos, 0f);
    }
}
