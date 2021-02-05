using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    [SerializeField] PlayerRegister register;

    private Vector3 checkpoint;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            register.PlayerOne.GetComponent<Movement>().Stop();
            register.PlayerOne.transform.position = register.SpawnPoint;
        }
    }
}
