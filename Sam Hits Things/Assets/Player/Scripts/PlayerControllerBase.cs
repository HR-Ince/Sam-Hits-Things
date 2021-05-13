using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{

    [SerializeField] protected PlayerStateRegister register;
    [SerializeField] protected GameEvent OnPlayerDeath;
    [SerializeField] protected LayerMask playerLayerMask;

    public bool IsGrounded { get { return isGrounded; } }

    protected bool isGrounded = true;
    protected float gravityMagnitude;
    protected Vector3 colliderCentre;
    protected Vector3 colliderHalfExtents;

    protected Collider _collider;
    protected LineDrawer line;
    protected LaunchModule launcher;
    protected PlayerInput input;
    protected PlayerTargettingManager targetting;
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        FetchExternalVariables();

        gravityMagnitude = Vector3.Magnitude(Physics.gravity);
    }
    protected void FetchExternalVariables()
    {
        input = GetComponent<PlayerInput>();

        _collider = GetComponent<Collider>();
        if (_collider == null) { Debug.LogError("Collider missing from player"); }
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) { Debug.LogError("Rigidbody missing from player"); }

        launcher = GetComponentInChildren<LaunchModule>();
        if (launcher == null) { Debug.LogError("Launcher missing from player"); }
        line = GetComponentInChildren<LineDrawer>();
        if (line == null) { Debug.LogError("LineDrawer missing from children of player"); }
        targetting = GetComponentInChildren<PlayerTargettingManager>();
        if (targetting == null) { Debug.LogError("Targetting Manager missing from children of player"); }
    }
    protected void GetIsGrounded()
    {
        if (Physics.CheckBox(transform.position + colliderCentre - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x, 0.1f, colliderHalfExtents.z), Quaternion.identity, ~playerLayerMask))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    public bool GetIsGroundedNarrow()
    {
        if (Physics.CheckBox(transform.position + colliderCentre - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x / 2, 0.1f, colliderHalfExtents.z / 2), Quaternion.identity, ~playerLayerMask))
            return true;
        else
            return false;
    }
    public void BroadcastDeath()
    {
        OnPlayerDeath.Invoke();
    }
    protected void SortFacing(Vector3 direction)
    {
        if (direction.x > 0.00f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0.00f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
