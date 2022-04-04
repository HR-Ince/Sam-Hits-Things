using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EarthBreakable : MonoBehaviour
{
    [SerializeField] float _velocityToBreak;
    [SerializeField] GameObject _brokenObj;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > _velocityToBreak)
        {
            print(collision.relativeVelocity.magnitude);
            Break();
        }
            
    }

    private void Break()
    {
        _brokenObj.SetActive(true);
        gameObject.SetActive(false);
    }
}
