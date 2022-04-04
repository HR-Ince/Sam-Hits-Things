using UnityEngine;
using UnityEngine.Events;

public class VesselStateManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnInactive;

    // Public variables
    public bool IsActive { get { return _isActive; } }
    public bool IsGrounded { get { return _isGrounded; } }

    // Private variables
    private bool _isActive;
    private bool _isGrounded;

    // Component references
    private Rigidbody _rb;
    private SphereCollider _sCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        TrackActiveThroughVelocity();
    }

    private void TrackActiveThroughVelocity()
    {
        if (_rb.velocity.magnitude > 0) _isActive = true;
        else if (_rb.velocity.magnitude <= 0 && _isActive) OnInactive.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isGrounded = CheckForGround();
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = CheckForGround();
    }

    private bool CheckForGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, _sCollider.radius + 0.1f);
    }

    public void ResetActive()
    {
        _isActive = false;
    }

    private void OnDisable()
    {
        _isActive = false;
        OnInactive.Invoke();
    }
}
