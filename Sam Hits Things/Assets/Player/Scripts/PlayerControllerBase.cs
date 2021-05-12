using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{

    [SerializeField] PlayerStateRegister register;
    [SerializeField] GameEvent OnPlayerDeath;
    [SerializeField] LayerMask playerLayerMask;

    public bool IsGrounded { get { return isGrounded; } }

    internal bool isGrounded = true;
    internal Vector3 colliderHalfExtents;

    internal BoxCollider _collider;
    internal LineDrawer line;
    internal LaunchModule launcher;
    internal PlayerInput input;
    internal PlayerTargettingManager targetting;
    internal Rigidbody _rigidbody;

    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;
        colliderHalfExtents = new Vector3(_collider.size.x / 2, _collider.size.y / 2, _collider.size.z / 2);
    }
    internal void FetchExternalVariables()
    {
        input = GetComponent<PlayerInput>();

        _collider = GetComponent<BoxCollider>();
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
    internal void GetIsGrounded()
    {
        if (Physics.CheckBox(transform.position + _collider.center - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x, 0.1f, colliderHalfExtents.z), Quaternion.identity, ~playerLayerMask))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    public bool GetIsGroundedNarrow()
    {
        if (Physics.CheckBox(transform.position + _collider.center - new Vector3(0, colliderHalfExtents.y, 0),
            new Vector3(colliderHalfExtents.x / 2, 0.1f, colliderHalfExtents.z / 2), Quaternion.identity, ~playerLayerMask))
            return true;
        else
            return false;
    }
    public void BroadcastDeath()
    {
        OnPlayerDeath.Invoke();
    }
    internal void SortFacing(Vector3 direction)
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
