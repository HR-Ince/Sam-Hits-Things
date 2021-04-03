using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    [SerializeField] PlayerRegister register;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            register.PlayerOne.GetComponent<Movement>().Stop();
            register.PlayerOne.transform.position = register.SpawnPoint;
        }
    }
}
