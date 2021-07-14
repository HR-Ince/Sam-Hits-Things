using UnityEngine;

public class DemonAirFormMod : MonoBehaviour
{
    [SerializeField] float airFormFallSpeed;

    private Rigidbody rB;


    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rB.velocity.y < 0f && Mathf.Abs(rB.velocity.y) > airFormFallSpeed)
        {
            rB.velocity = new Vector3(rB.velocity.x, -airFormFallSpeed, 0);
        }

    }
}
