using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour, IPerishable
{
    //public enum Facing { left, right }
    [SerializeField] PlayerStateRegister register;
    [SerializeField] GameEvent OnPlayerDeath;
    [SerializeField] LayerMask playerLayerMask;

    public bool IsBurdened { get { return isBurdened; } }
    public bool IsGrounded { get { return isGrounded; } }
    
    [SerializeField] private bool isBurdened = false;
    [SerializeField] private bool isGrounded = true;

    Vector3 colliderHalfExtents;

    private BoxCollider _collider;

    private void Awake()
    {
        register.PlayerOne = gameObject;
        _collider = GetComponent<BoxCollider>();

        colliderHalfExtents = new Vector3(_collider.size.x / 2, _collider.size.y / 2, _collider.size.z / 2);
    }
    private void Update()
    {
        if(Physics.CheckBox(transform.position + _collider.center - new Vector3(0, colliderHalfExtents.y, 0), new Vector3(colliderHalfExtents.x, 0.1f, colliderHalfExtents.z), Quaternion.identity, ~playerLayerMask))
            isGrounded = true;
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
    public void SetIsBurdened(bool value)
    {
        isBurdened = value;
    }
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
    }
}
