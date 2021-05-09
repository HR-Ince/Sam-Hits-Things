using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] float percentageOfTrajectoryToDraw = 0.75f;
    private LineRenderer line;
    private List<Vector3> linePoints = new List<Vector3>();
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
        line.positionCount = 10;
        line.enabled = false;
    }

    public void StartLine(Vector3 objectPosition)
    {
        line.SetPosition(0, objectPosition);
    }
    public void ManageTrajectoryLine(float objectMass)
    {        
        if (float.IsNaN(launcher.DirectionVector.x) || float.IsNaN(launcher.DirectionVector.y)) { return; }
        if (line.enabled == false) { line.enabled = true; }

        Vector3 forceVector = launcher.DirectionVector * (launcher.DrawPercentage * launcher.ThrustModifier);
        Vector3 velocityVector = (forceVector / objectMass) * Time.fixedDeltaTime;

        float gravity = Vector3.Magnitude(Physics.gravity);
        float flightTime = Vector3.Magnitude(velocityVector) / (gravity / 2);

        float stepInterval = flightTime * percentageOfTrajectoryToDraw / line.positionCount;

        linePoints.Clear();

        float stepTimePassed;
        Vector3 startingPos = transform.position;
        float facing;
        if(velocityVector.x > 0)
            facing = transform.right.x;
        else
            facing = -transform.right.x;
        

        for(int i = 0; i < line.positionCount; i++)
        {
            stepTimePassed = stepInterval * i;

            float xPos = startingPos.x + facing * velocityVector.x * stepTimePassed;
            float yPos = startingPos.y + velocityVector.y * stepTimePassed - 0.5f * gravity * stepTimePassed * stepTimePassed;

            linePoints.Add(new Vector3(xPos, yPos, 0));
        }
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }
    public void DisableLine()
    {
        line.enabled = false;
    }
}
