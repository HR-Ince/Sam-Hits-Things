using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private enum Direction { left, right, up, down }

    [SerializeField] Direction beamDirection;
    [SerializeField] PlayerStateRegister register;

    private Vector3 direction;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponentInChildren<LineRenderer>();
        if(line == null) { Debug.LogError("No Line Renderer in children of " + name);  }

        if (beamDirection == Direction.down)
            direction = Vector3.down;
        else if (beamDirection == Direction.left)
            direction = Vector3.left;
        else if (beamDirection == Direction.right)
            direction = Vector3.right;
        else if (beamDirection == Direction.up)
            direction = Vector3.up;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        if(Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity))
        {            
            UpdateLineRenderer(hit.point - transform.position);
            
            if(hit.transform.TryGetComponent(out IPerishable comp))
            {
                comp.BroadcastDeath();
            }
        }
    }

    void UpdateLineRenderer(Vector3 hitPoint)
    {
        line.SetPosition(1, hitPoint);
    }
}
