using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class OriballPlayerController : MonoBehaviour, IPerishable
{
    [SerializeField] PlayerStateRegister register;
    [SerializeField] GameEvent OnPlayerDeath;
    [SerializeField] LayerMask playerLayerMask;

    public bool IsGrounded { get { return isGrounded; } }

    private bool isGrounded = true;
    private Vector3 colliderHalfExtents;

    private Collider _collider;
    private LineDrawer line;
    private ManualLauncher launcher;
    private OrbitTargeter orbit;
    private PlayerInput input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;
        if (_collider is BoxCollider box)
            colliderHalfExtents = new Vector3(box.size.x / 2, box.size.y / 2, box.size.z / 2);
        else if (_collider is SphereCollider sphere)
            colliderHalfExtents = new Vector3(sphere.radius, sphere.radius, sphere.radius);
    }
    private void FetchExternalVariables()
    {
        input = GetComponent<PlayerInput>();

        _collider = GetComponent<Collider>();
        if (_collider == null) { Debug.LogError("Collider missing from player"); }
        launcher = GetComponent<ManualLauncher>();
        if (launcher == null) { Debug.LogError("Launcher missing from player"); }
        orbit = GetComponent<OrbitTargeter>();
        if(orbit == null) { Debug.LogError("OrbitController missing from player"); }
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) { Debug.LogError("Rigidbody missing from player"); }

        line = GetComponentInChildren<LineDrawer>();
        if (line == null) { Debug.LogError("LineDrawer missing from children of player"); }
    }

    private void Update()
    {
        if (Physics.CheckBox(_collider.transform.position - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x, 0.1f, colliderHalfExtents.z), Quaternion.identity, ~playerLayerMask))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;

        if (isGrounded)
        {
            if (input.Pressed)
            {
                if (orbit.HasOrbitTarget())
                {
                    Vector3 orbitCentre = orbit.GetOrbitCentre();
                    line.StartLineAt(orbitCentre);
                }
                else
                    line.StartLineAt(transform.position);

                launcher.SetObjectToLaunch(gameObject);
                launcher.SetPressPos(input.PressPos);
            }

            if (input.PressHeld)
            {
                launcher.AdjustTargetting(input.PressPos);
                if (orbit.Target != null)
                {
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.useGravity = false;
                    if (launcher.WillLaunch())
                        line.ManageBasicLine(launcher.DirectionVector, launcher.DrawPercentage);
                }
                else if(launcher.WillLaunch())
                    line.ManageTrajectoryLine(launcher.DirectionVector, launcher.DrawPercentage, launcher.ThrustModifier, _rigidbody.mass);
                print(launcher.DrawPercentage);
            }

            if (input.PressReleased)
            {
                _rigidbody.useGravity = true;
                line.DisableLine();
                if (launcher.WillLaunch())
                {
                    if (orbit.Target != null)
                    {
                        transform.position = orbit.GetLaunchFromPosition(launcher.DirectionVector);
                        launcher.Launch(Vector3.Magnitude(Physics.gravity));
                    }
                    else
                    {
                        launcher.Launch(1);
                    }
                        
                }
                orbit.ClearTarget();
            }

        }
    }
    public bool GetIsGroundedNarrow()
    {
        if (Physics.CheckBox(_collider.transform.position - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x / 2, 0.1f, colliderHalfExtents.z / 2), Quaternion.identity, ~playerLayerMask))
            return true;
        else
            return false;
    }
    public void BroadcastDeath()
    {
        OnPlayerDeath.Invoke();
    }
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
    }
}
