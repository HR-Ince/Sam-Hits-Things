using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour, IPerishable
{
    [SerializeField] PlayerStateRegister register;
    [SerializeField] GameEvent OnPlayerDeath;

    public bool IsBurdened { get { return isBurdened; } }
    public bool IsStopped { get { return isStopped; } set { isStopped = value; } }

    private bool isBurdened = false;
    private bool isStopped = true;

    private void OnEnable()
    {
        register.PlayerOne = gameObject;
    }
    public void BroadcastDeath()
    {
        OnPlayerDeath.Invoke();
    }
    public void SetIsBurdened(bool value)
    {
        isBurdened = value;
    }
}
