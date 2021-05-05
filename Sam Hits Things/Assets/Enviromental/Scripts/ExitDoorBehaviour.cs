using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitDoorBehaviour : MonoBehaviour
{
    [SerializeField] WorldState worldState;
    [SerializeField] GameEvent onExit;
    [SerializeField] PlayerStateRegister register;

    private PlayerController playerState;

    private void Awake()
    {
        playerState = register.PlayerOne.GetComponent<PlayerController>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (worldState.InGame == false) { return; }

        if(playerState.IsGrounded)
            ExitState();   
    }
    private void ExitState()
    {
        worldState.InGame = false;

        if (onExit != null)
            onExit.Invoke(); 
    }
}
