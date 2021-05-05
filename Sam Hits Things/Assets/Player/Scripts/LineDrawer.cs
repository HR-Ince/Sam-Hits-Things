using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;

    private LineRenderer line;
    private ManualLauncher launcher;

    private void Awake()
    {
        FetchExternalVariables();
        SetLineVariables();
    }
    private void FetchExternalVariables()
    {
        line = GetComponent<LineRenderer>();
        launcher = GetComponentInParent<ManualLauncher>();
        if (launcher == null) { Debug.LogError("ManualLauncher missing from parent of LineDrawer"); }
    }
    private void SetLineVariables()
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
        line.positionCount = 2;
    }

    public void StartLine(Vector3 objectPosition)
    {
        line.SetPosition(0, objectPosition);
        line.enabled = true;
    }
    public void ManageLine()
    {
        line.SetPosition(1, launcher.TargetVector);
        SetLineColour(launcher.DrawPercentage);
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
