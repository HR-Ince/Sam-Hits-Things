using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] float percentageOfTrajectoryToDraw = 0.75f;

    private PlayerTargettingManager targeter;
    private LineRenderer line;
    private List<Vector3> linePoints = new List<Vector3>();

    private void Awake()
    {
        FetchExternalVariables();
        SetLineVariables();
    }
    private void FetchExternalVariables()
    {
        line = GetComponent<LineRenderer>();
        targeter = GetComponent<PlayerTargettingManager>();
        if(targeter == null) { Debug.LogError("Targeter missing from line drawer GO"); }
    }
    private void SetLineVariables()
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 10;
        line.enabled = false;
    }
    public void ManageTrajectoryLine(float thrustModifier, float additionalThrust, float mass, ForceMode forceMode)
    {
        Vector3 targetDirection = targeter.DirectionVector;

        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        line.enabled = true;

        float forceDuration = forceMode == ForceMode.Force || forceMode == ForceMode.Acceleration ? Time.fixedDeltaTime : 1;
        float objectMass = forceMode == ForceMode.Impulse || forceMode == ForceMode.Force ? mass : 1;

        float thrust = targeter.DrawPercentage * (thrustModifier + additionalThrust);
        Vector3 forceVector = targetDirection * thrust;
        Vector3 velocityVector = (forceVector / objectMass) * forceDuration;

        float gravity = Vector3.Magnitude(Physics.gravity);
        float flightTime = Vector3.Magnitude(velocityVector) / (gravity / 2);

        float stepInterval = flightTime * percentageOfTrajectoryToDraw / line.positionCount;

        linePoints.Clear();

        float stepTimePassed;
        Vector3 startingPos = targeter.TargettingOrigin;

        for(int i = 0; i < line.positionCount; i++)
        {
            stepTimePassed = stepInterval * i;

            float xPos = startingPos.x + velocityVector.x * stepTimePassed;
            float yPos = startingPos.y + velocityVector.y * stepTimePassed - 0.5f * gravity * stepTimePassed * stepTimePassed;

            linePoints.Add(new Vector3(xPos, yPos, 0));
        }
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }
    public void ManageBasicLine(Vector3 targetDirection, float length)
    {
        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        line.enabled = true;
        Vector3 targetVector = Vector3.ClampMagnitude(targetDirection * length, 5);
        targetVector += line.GetPosition(0);

        for (int i = 1; i < line.positionCount; i++)
        {
            float interval = (float)i / line.positionCount;
            line.SetPosition(i, Vector3.Lerp(line.GetPosition(0), targetVector, interval));
        }
    }
    public void DisableLine()
    {
        line.enabled = false;
    }
}
