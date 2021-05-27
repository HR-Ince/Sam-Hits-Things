using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class SpiderPlayerController : PlayerControllerBase, IPerishable
{
    [SerializeField] float landingVelocity = 8f;

    private bool isAtLandingVelocity;
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
        TrackVelocity();

        if (isGrounded)
        {
            if (input.Pressed)
            {
                ProcessPress();
            }
            if (pressAccepted && input.PressHeld)
            {
                ProcessPressHeld();
            }
            if (pressAccepted && input.PressReleased)
            {
                ProcessPressReleased();
            }
        }
        else
        {
            if (orbitTargetting.HasOrbitTarget())
            {
                if (input.Pressed)
                {
                    ProcessPressWithOrbit();
                }
                if (pressAccepted && input.PressHeld)
                {
                    orbitTargetting.AdjustTargetting(input.PressPos);
                    if (orbitTargetting.DrawIsSufficient())
                    {
                        line.ManageTrajectoryLine(jumpStrength, gravityMagnitude, _rigidbody.mass, ForceMode.Force); // Additional thrust variable needed
                        line.enabled = true;
                    }
                }
                if (pressAccepted && input.PressReleased)
                {
                    line.DisableLine();
                    Physics.IgnoreCollision(_collider, orbitTargetting.Target.GetComponent<Collider>());
                    _rigidbody.useGravity = true;
                    launcher.Launch(_rigidbody, orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, jumpStrength + gravityMagnitude, ForceMode.Force);
                    if (orbitTargetting.IsTargetMovable)
                    {
                        Rigidbody targetRB = orbitTargetting.Target.GetComponent<Rigidbody>();
                        if (targetRB.isKinematic)
                            targetRB.isKinematic = false;
                        launcher.Launch(targetRB, -orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, jumpStrength + gravityMagnitude, ForceMode.Force);
                    }

                    orbitTargetting.ClearTarget();
                    pressAccepted = false;
                }
            }
        }
    }

    private void ProcessPressWithOrbit()
    {
        ProcessPress();

        _rigidbody.useGravity = false;

        orbitTargetting.GetOrbitTarget();
    }

    private void ProcessPressReleased()
    {
        line.DisableLine();
        if (orbitTargetting.DrawIsSufficient())
        {
            launcher.Launch(_rigidbody, orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, jumpStrength, ForceMode.Force);
        }

        pressAccepted = false;
    }

    private void ProcessPressHeld()
    {
        orbitTargetting.AdjustTargetting(input.PressPos);
        if (orbitTargetting.DrawIsSufficient())
        {
            line.ManageTrajectoryLine(jumpStrength, 0f, _rigidbody.mass, ForceMode.Force);
            line.enabled = true;
        }
        else
            line.enabled = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isAtLandingVelocity) { return; }
        if (collision.gameObject.layer == floorLayer || collision.gameObject.layer == wallLayer)
        {
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
        }
    }
    private void TrackVelocity()
    {
        float velocity = Vector3.Magnitude(_rigidbody.velocity);

        if (velocity <= landingVelocity)
            isAtLandingVelocity = true;
        else
            isAtLandingVelocity = false;

        if (velocity <= Mathf.Epsilon)
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
}
