using UnityEngine;

public class MouseOverInterfaceChecker : MonoBehaviour
{
    private float camDepth;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        camDepth = Mathf.Abs(cam.transform.position.z);
    }

    public bool GetIsMouseOverInterfaceObj()
    {
        if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, camDepth * 2))
        {
            if (hit.transform.gameObject.TryGetComponent(out PlayerInput input))
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
