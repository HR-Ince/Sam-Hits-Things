using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetHandler : MonoBehaviour
{
    private Vector3 originPos;

    private void Awake()
    {
        SetCheckpoint();
    }
    public void SetCheckpoint()
    {
        originPos = transform.position;
    }
    public void ResetPosition()
    {
        if (TryGetComponent(out Rigidbody rb))
            rb.velocity = Vector3.zero;
        transform.position = originPos;
    }
}
