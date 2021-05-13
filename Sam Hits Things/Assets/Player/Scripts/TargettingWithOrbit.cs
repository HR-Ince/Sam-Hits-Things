using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingWithOrbit : PlayerTargettingManager
{
    [SerializeField] float orbitRadius = 2f;
    [SerializeField] LayerMask interactableMask;

    public bool IsTargetMovable { get 
        { 
            if(target == null) { return false; }
            else { return target.CompareTag("Movable"); }
        } }
    public GameObject Target { get { return target; } }

    private Collider[] targets;

    private float targetDistance;
    private GameObject target;

    public bool HasOrbitTarget()
    {
        targets = Physics.OverlapSphere(transform.position, orbitRadius, interactableMask);

        if (targets.Length <= 0)
            return false;
        else
            return true;
    }

    public void GetOrbitCentre()
    {
        foreach (Collider collider in targets)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (target == null || distance < targetDistance)
            {
                target = collider.gameObject;
                targetDistance = distance;
            }
        }

        targettingOrigin = target.transform.position;
    }
    public new void AdjustTargetting(Vector3 inputPos)
    {
        base.AdjustTargetting(inputPos);
    }
    public void ClearTarget()
    {
        Time.timeScale = 1f;
        target = null;
    }

}
