using UnityEngine;

public class OrbitTargeter : MonoBehaviour
{
    [SerializeField] float oribtRadius = 2f;
    [SerializeField] LayerMask interactableMask;

    public bool TargetIsMovable { get
        {
            if (target.CompareTag("Movable"))
                return true;
            else
                return false;
        } }
    public bool IsOribiting { get { return isOrbiting; } }
    public GameObject Target { get { return target; } }

    private Collider[] targets;

    private bool isOrbiting;
    private float targetDistance;
    private GameObject target;
    private Vector3 orbitCentre;

    public bool HasOrbitTarget()
    {
        targets = Physics.OverlapSphere(transform.position, oribtRadius, interactableMask);

        if (targets.Length <= 0)
            return false;
        else
            return true;
    }

    public Vector3 GetOrbitCentre()
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
            orbitCentre = Vector3.Lerp(transform.position, target.transform.position, 0.5f);
        }
        else
        {
            orbitCentre = target.transform.position;
        }

        return orbitCentre;
            
    }
    public Vector3 GetLaunchFromPosition(Vector3 direction)
    {
        if(float.IsNaN(direction.x) || float.IsNaN(direction.y) || target == null) { return Vector3.zero; }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        float targetAngle = Vector3.SignedAngle(Vector3.right, direction, Vector3.back);

        float newX = orbitCentre.x + targetDistance * Mathf.Cos(targetAngle);
        float newY = orbitCentre.y + targetDistance * Mathf.Sin(targetAngle);

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
