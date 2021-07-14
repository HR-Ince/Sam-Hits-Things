using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class LaunchData : MonoBehaviour
{
    [SerializeField] float gravityModifier = 1f;

    public float GravityModifier { get { return gravityModifier; } }
    public Rigidbody Rigidbody { get { return rB; } }

    private Rigidbody rB;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }

    public float GetMass()
    {
        return rB.mass;
    }
}
