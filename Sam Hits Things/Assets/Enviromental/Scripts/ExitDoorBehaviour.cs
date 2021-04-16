using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitDoorBehaviour : MonoBehaviour
{
    [SerializeField] WorldState worldState;
    [SerializeField] GameEvent onExit;

    private void OnTriggerStay(Collider other)
    {
        if (worldState.InGame == false) { return; }

        if(GetGrounded(other))
            ExitState();        
    }
    private bool GetGrounded(Collider other)
    {
        GroundingCheck gC = other.GetComponentInChildren<GroundingCheck>();
        if (gC == null) { print("No GC"); return false; }

        if (gC.IsGrounded)
            return true; 
        else
            return false;
    }
    private void ExitState()
    {
        worldState.InGame = false;

        if (onExit != null)
            onExit.Invoke(); 
    }
}
