using UnityEngine;
using UnityEngine.UI;

public class AngleFinder : MonoBehaviour
{
    [SerializeField] Image press, input;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            press.transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            input.transform.position = Input.mousePosition;
        }

        Vector3 direction = press.transform.position - input.transform.position;
        float angle = Vector3.SignedAngle(Vector3.right, direction.normalized, Vector3.back);
        print(angle);
    }
}
