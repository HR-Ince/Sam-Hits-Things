using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
        line.positionCount = 2;
    }
    public void SetLinePositions(Vector3 playerPosition, Vector3 targetVector)
    {
        line.enabled = true;

        line.SetPosition(0, playerPosition);
        line.SetPosition(1, targetVector);
    }
    public void SetLineColour(float drawPercentage)
    {
        Color lineColor = Color.Lerp(Color.black, Color.red, drawPercentage / 100);
        line.startColor = lineColor;
        line.endColor = lineColor;
    }
    public void DisableLine()
    {
        line.enabled = false;
    }
}
