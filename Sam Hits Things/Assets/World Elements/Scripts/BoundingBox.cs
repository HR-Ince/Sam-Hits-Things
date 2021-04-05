using Cinemachine;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    private Collider box;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        box = GetComponent<Collider>();

        float depth = cam.transform.position.z;
        float height = depth * 2f * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2f);
        
    }
}
