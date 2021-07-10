using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class LaunchData : MonoBehaviour
{
    [SerializeField] float gravityModifier = 1f;

    public float GravityModifier { get { return gravityModifier; } }
    public float Mass { get { return mass; } }
    public Rigidbody Rigidbody { get { return rB; } }

    private float mass;

    private Rigidbody rB;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        mass = rB.mass;
    }
}
