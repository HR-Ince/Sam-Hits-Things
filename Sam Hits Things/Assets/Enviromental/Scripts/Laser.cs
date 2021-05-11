using UnityEngine;

public class Laser : MonoBehaviour
{
    private enum Direction { left, right, up, down }
    [SerializeField] Gradient color;
    [SerializeField] Direction beamDirection;
    [SerializeField] private ParticleSystem burn, source;
    private Vector3 direction;

    private LineRenderer line;
    

    private void Awake()
    {
        FetchExternalVariables();
        SetupLine();
        SetupParticles();
    }
    private void FetchExternalVariables()
    {
        line = GetComponentInChildren<LineRenderer>();
        if (line == null) { Debug.LogError("No Line Renderer in children of " + name); }
    }
    private void SetupLine()
    {
        if (beamDirection == Direction.down)
            direction = Vector3.down;
        else if (beamDirection == Direction.left)
            direction = Vector3.left;
        else if (beamDirection == Direction.right)
            direction = Vector3.right;
        else if (beamDirection == Direction.up)
            direction = Vector3.up;

        line.SetPosition(1, direction * 5f);
        line.colorGradient = color;
    }
    private void SetupParticles()
    {
        if(burn != null)
        {
            var colorOverLifetime = burn.colorOverLifetime;
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(color);
        }
        
    }
    void Update()
    {
        Debug.DrawRay(transform.position, direction, Color.red, 100f);
        if(Physics.Raycast(transform.position, direction, out RaycastHit hit, 100f))
        {
            line.SetPosition(1, direction * hit.distance);
            
            if (hit.transform.TryGetComponent(out IPerishable comp))
            {
                comp.BroadcastDeath();
            }
        }

        source.transform.position = transform.position;
        burn.transform.localPosition = line.GetPosition(1);
    }
}
