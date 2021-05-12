using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingWithOrbit : PlayerTargettingManager
{
    [SerializeField] float orbitRadius = 2f;
    [SerializeField] LayerMask interactableMask;

    public GameObject Target { get { return target; } }

    private Collider[] targets;

    private bool isOrbiting; 
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

        if (target.CompareTag("Movable"))
        {
            targettingOrigin = Vector3.Lerp(transform.position, target.transform.position, 0.5f);
        }
        else
        {
            targettingOrigin = target.transform.position;
        }
    }
    public new void AdjustTargetting(Vector3 inputPos)
    {
        base.AdjustTargetting(inputPos);
        GetLaunchPosition();
    }

    private Vector3 GetLaunchPosition()
    {
        if (float.IsNaN(dirVector.x) || float.IsNaN(dirVector.y) || target == null) { return Vector3.zero; }

        float targetAngle = Vector3.SignedAngle(Vector3.right, dirVector, Vector3.back);

        float newX = targettingOrigin.x + targetDistance * Mathf.Cos(targetAngle);
        float newY = targettingOrigin.y + targetDistance * Mathf.Sin(targetAngle);

        isOrbiting = true;

        return new Vector3(newX, newY, 0);
    }
    public void ClearTarget()
    {
        Time.timeScale = 1f;
        target = null;
        isOrbiting = false;
    }

}
