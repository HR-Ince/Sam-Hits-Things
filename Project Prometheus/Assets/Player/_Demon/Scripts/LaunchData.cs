using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class LaunchData : MonoBehaviour
{
    [SerializeField] float gravityModifier = 1f;

    public float GravityModifier { get { return gravityModifier; } }
    public float DefaultDrag { get { return defaultDrag; } }
    public Rigidbody Rigidbody { get { return rB; } }

    private Rigidbody rB;
    private float defaultDrag;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        defaultDrag = rB.drag;
    }

    public float GetMass()
    {
        return rB.mass;
    }
}
