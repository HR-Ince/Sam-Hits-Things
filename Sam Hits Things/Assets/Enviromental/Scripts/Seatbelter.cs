using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seatbelter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.parent = transform.parent;
    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.parent = null;
    }
}
