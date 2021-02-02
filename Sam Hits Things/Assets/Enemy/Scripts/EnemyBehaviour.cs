using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] PlayerRegister register;
    [SerializeField] WorldStats stats;

    private bool isDead = false;

    private void Awake()
    {
        stats.EnemiesInLevel++;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) { return; }
        if (other.gameObject == register.PlayerOne)
            ProcessStrike();
    }
    private void ProcessStrike()
    {
        Die();
    }
    private void Die()
    {
        stats.EnemiesInLevel--;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        isDead = true;
    }
}
