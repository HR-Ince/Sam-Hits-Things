using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] PlayerRegister register;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == register.PlayerOne)
            ProcessStrike();
    }
    private void ProcessStrike()
    {
        Die();
    }
    private void Die()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
}
