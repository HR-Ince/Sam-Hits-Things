using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WindOrganiser : MonoBehaviour
{
    [SerializeField] float windRelativeSpeed = 0.7f;

    private BoxCollider _collider;
    private ParticleSystem particles;

    private void Awake()
    {
        FetchExternalVariables();
        ShapeCalculations();
        MainCalculations();
    }

    private void FetchExternalVariables()
    {
        _collider = GetComponentInParent<BoxCollider>();
        particles = GetComponent<ParticleSystem>();
    }

    private void ShapeCalculations()
    {
        if (_collider == null) { Debug.LogError("Collider missing from Wind Tunnel"); return; }

        var shape = particles.shape;
        
        Vector3 shapeScale = new Vector3(_collider.size.x, 0.1f, _collider.size.z);
        Vector3 shapePos = new Vector3(0, -(_collider.size.y / 2), 0);

        shape.scale = shapeScale;
        shape.position = shapePos;
    }

    private void MainCalculations()
    {
        var main = particles.main;

        float colliderY = _collider.size.y;

        main.startLifetime = colliderY;
        main.simulationSpeed = colliderY * windRelativeSpeed;
    }
}
