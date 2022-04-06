using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WaterTransition : MonoBehaviour
{
    [SerializeField] private Transform _exit;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = _exit.position;
    }
}
