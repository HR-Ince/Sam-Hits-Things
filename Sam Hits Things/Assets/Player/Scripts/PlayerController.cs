﻿using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IPerishable
{
    [SerializeField] PlayerStateRegister register;
    [SerializeField] GameEvent OnPlayerDeath;
    [SerializeField] UnityEvent onJump;
    [SerializeField] LayerMask playerLayerMask;

    public bool IsBurdened { get { return isBurdened; } }
    public bool IsGrounded { get { return isGrounded; } }

    private bool isBurdened = false;
    private bool isGrounded = true;
    private GameObject heldObject;
    private Vector3 colliderHalfExtents;

    private BoxCollider _collider;
    private LineDrawer line;
    private ManualLauncher launcher;
    private PlayerGrabHandler grabHandler;
    private PlayerInput input;

    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;
        colliderHalfExtents = new Vector3(_collider.size.x / 2, _collider.size.y / 2, _collider.size.z / 2);
    }
    private void FetchExternalVariables()
    {
        input = GetComponent<PlayerInput>();

        _collider = GetComponent<BoxCollider>();
        if (_collider == null) { Debug.LogError("Collider missing from player"); }
        launcher = GetComponent<ManualLauncher>();
        if (launcher == null) { Debug.LogError("Launcher missing from player"); }

        line = GetComponentInChildren<LineDrawer>();
        if (line == null) { Debug.LogError("LineDrawer missing from children of player"); }
        grabHandler = GetComponentInChildren<PlayerGrabHandler>();
        if (grabHandler == null) { Debug.LogError("GrabHandler missing from children of player"); }
    }

    private void Update()
    {
        if (grabHandler == null) { Debug.LogError("GrabHandler missing from children of player"); return; }
        if (Physics.CheckBox(transform.position + _collider.center - new Vector3(0, colliderHalfExtents.y, 0), 
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
                if (grabHandler.IsBurdened)
                {
                    launcher.SetObjectToLaunch(grabHandler.HeldObject);
                    line.StartLine(grabHandler.HeldObject.transform.position);
                }
                else
                {
                    launcher.SetObjectToLaunch(gameObject);
                    line.StartLine(transform.position);
                }
                
                launcher.SetPressPos(input.PressPos);
                SortFacing(input.PressPos);
            }

            if (input.PressHeld)
            {
                launcher.AdjustTargetting(input.PressPos);
                line.ManageLine();
                SortFacing(launcher.DirectionVector);
            }

            if (input.PressReleased)
            {
                line.DisableLine();
                if (launcher.WillLaunch())
                {
                    if (onJump != null && !grabHandler.IsBurdened)
                        onJump.Invoke();
                    if (grabHandler.IsBurdened)
                        grabHandler.ReleaseHeldObject();
                    launcher.Launch();
                }
                    
            }
                
        }
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
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
    }
    private void SortFacing(Vector3 direction)
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