using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] int percentageOfTrajectoryToDraw = 10;
    [SerializeField] int linePointsPerUnit = 10;
    [SerializeField] LayerMask wallMask;

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
        if (targeter == null) { Debug.LogError("Targeter missing from line drawer GO"); }
    }
    private void SetLineVariables()
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
    }
    public void ManageTrajectoryLine(LaunchData launchData, float thrust, ForceMode forceMode)
    {
        Vector3 targetDirection = targeter.DirectionVector;

        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        line.enabled = true;
        
        float forceDuration = forceMode == ForceMode.Force || forceMode == ForceMode.Acceleration ? Time.fixedDeltaTime : 1;
        float objectMass = forceMode == ForceMode.Impulse || forceMode == ForceMode.Force ? launchData.Mass : 1;
        
        Vector3 forceVector = targetDirection * (targeter.DrawPercentage * thrust);
        Vector3 velocityVector = (forceVector / objectMass) * forceDuration;
        

        float gravity = -Physics.gravity.y * launchData.GravityModifier;
        float flightTime = Vector3.Magnitude(velocityVector) / (gravity / 2);
        line.positionCount = Mathf.FloorToInt(flightTime * linePointsPerUnit);
        float stepInterval = flightTime / line.positionCount;

        linePoints.Clear();
        
        float stepTimePassed;
        Vector3 startingPos = transform.position;
        
        //Calculate full trajectory
        for(int i = 0; i < line.positionCount; i++)
        {
            stepTimePassed = stepInterval * i;

            float xPos = startingPos.x + velocityVector.x * stepTimePassed;
            float yPos = startingPos.y + velocityVector.y * stepTimePassed - 0.5f * gravity * stepTimePassed * stepTimePassed;

            Vector3 newPoint = new Vector3(xPos, yPos, 0);
            
            linePoints.Add(newPoint);
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
