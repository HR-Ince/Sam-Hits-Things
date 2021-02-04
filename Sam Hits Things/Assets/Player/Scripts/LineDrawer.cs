using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] Gradient baseGradient;
    [SerializeField] Gradient maxedGradient;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
        line.positionCount = 4;
        line.colorGradient = baseGradient;
    }
    public void SetFirstPoint(Vector3 pos)
    {
        line.SetPosition(0, pos);
    }
    public void SetLinePositions(Vector3 targetVector, float drawPercentage)
    {
        line.enabled = true;
        line.SetPosition(3, targetVector);
        line.SetPosition(2, Vector3.Lerp(line.GetPosition(0), line.GetPosition(3), 0.5f));
        line.SetPosition(1, Vector3.Lerp(line.GetPosition(0), line.GetPosition(2), 0.5f));

        if (drawPercentage > 99.00f)
        {
            line.colorGradient = maxedGradient;
        }
        else
            line.colorGradient = baseGradient;
    }
    public void DisableLine()
    {
        line.enabled = false;
    }
}
