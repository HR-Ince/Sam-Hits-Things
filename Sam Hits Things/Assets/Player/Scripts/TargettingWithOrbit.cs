using UnityEngine;
using UnityEngine.UI;

public class TargettingWithOrbit : PlayerTargettingManager
{
    [SerializeField] float orbitRadius = 2f;
    [SerializeField] Image crosshairUI;
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

    private void Awake()
    {
        FetchExternalVariables(); // base

        crosshairUI.enabled = false;
    }
    private void Update()
    {
        if (target != null)
        {
            ManageUI();
            print("Has target");
        }            
        else
            DisableUI();
    }
    public bool HasOrbitTarget()
    {
        targets = Physics.OverlapSphere(transform.position, orbitRadius, interactableMask);

        if (targets.Length <= 0)
            return false;
        else
            return true;
    }
    public void GetOrbitTarget()
    {
        foreach (Collider collider in targets)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (target == null || distance < targetDistance)
            {
                target = collider.gameObject;
            }
        }

        FreezeOrbitTarget();
    }

    private void FreezeOrbitTarget()
    {
        if(target.TryGetComponent(out Rigidbody rB))
        {
            rB.isKinematic = true;
        }
    }
    public void ManageUI()
    {
        if (!crosshairUI.enabled)
            crosshairUI.enabled = true;

        crosshairUI.transform.position = cam.WorldToScreenPoint(target.transform.position);

        float angle = Vector3.SignedAngle(dirVector, Vector3.right, Vector3.back);
        Vector3 eulerRot = new Vector3(0f, 0f, angle);

        crosshairUI.transform.rotation = Quaternion.Euler(eulerRot);
    }
    public void DisableUI()
    {
        crosshairUI.enabled = false;
    }
    public void ClearTarget()
    {
        Time.timeScale = 1f;
        if(target.TryGetComponent(out Rigidbody rB))
        {
            rB.isKinematic = false;
        }
        target = null;
    }

}
