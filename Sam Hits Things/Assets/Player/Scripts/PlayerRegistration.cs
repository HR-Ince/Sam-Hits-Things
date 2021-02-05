using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistration : MonoBehaviour
{
    [SerializeField] PlayerRegister register;

    private void OnEnable()
    {
        register.PlayerOne = gameObject;
        register.SpawnPoint = transform.position;
    }
}
