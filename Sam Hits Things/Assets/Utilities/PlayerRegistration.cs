using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistration : MonoBehaviour
{
    [SerializeField] PlayerRegister playerRegister;

    private void Awake()
    {
        playerRegister.PlayerOne = gameObject;
    }
}
