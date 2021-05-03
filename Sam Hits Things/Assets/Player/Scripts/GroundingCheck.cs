using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingCheck : MonoBehaviour
{
    [SerializeField] float slipForce;
    [SerializeField] WorldState world;

    private PlayerStateManager state;

    private void Awake()
    {
        state = GetComponentInParent<PlayerStateManager>();
        if(state == null) { Debug.LogError("State Manager missing from Player"); }
    }
    private void OnTriggerEnter(Collider other)
    {
        state.SetIsGrounded(true);
    }
    private void OnTriggerExit(Collider other)
    {
        state.SetIsGrounded(false);
    }
}
