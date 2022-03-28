using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] int linePointsPerUnit = 10;
    [SerializeField] LayerMask playerMask;

    private Vector3 objectPos;
    private float objectMass;
    private float objectGravityMod;

    private PlayerDrawHandler targeter;
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
        targeter = GetComponent<PlayerDrawHandler>();
        if (targeter == null) { Debug.LogError("Targeter missing from line drawer GO"); }
    }
    private void SetLineVariables()
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
    }

    public void SetLaunchObjectVariables(LaunchData launchData)
    {
        objectPos = launchData.transform.position;
        objectMass = launchData.GetMass();
        objectGravityMod = launchData.GravityModifier;
    }

    public void ManageTrajectoryLine(float thrust, ForceMode forceMode)
    {
        Vector3 targetDirection = targeter.DirectionVector;

        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        line.enabled = true;
        
        float forceDuration = forceMode == ForceMode.Force || forceMode == ForceMode.Acceleration ? Time.fixedDeltaTime : 1;
        float mass = forceMode == ForceMode.Impulse || forceMode == ForceMode.Force ? objectMass : 1;
        
        Vector3 forceVector = targetDirection * (targeter.DrawPercentage * thrust);
        Vector3 velocityVector = (forceVector / mass) * forceDuration;
        

        float gravity = -Physics.gravity.y * objectGravityMod;
        float flightTime = Vector3.Magnitude(velocityVector) / (gravity / 2);
        float linePositions = Mathf.FloorToInt(flightTime * linePointsPerUnit);
        float stepInterval = flightTime / linePositions;

        linePoints.Clear();
        
        float stepTimePassed;
        Vector3 startingPos = objectPos;

        for (int i = 0; i < linePositions; i++)
        {
            stepTimePassed = stepInterval * i;

            float xPos = startingPos.x + velocityVector.x * stepTimePassed;
            float yPos = startingPos.y + velocityVector.y * stepTimePassed - 0.5f * gravity * stepTimePassed * stepTimePassed;
            Vector3 newPoint = new Vector3(xPos, yPos, 0);

            if (i == 0) 
            {
                linePoints.Add(newPoint);
                continue;
            }

            Vector3 previousPoint = linePoints[i - 1];

            Vector3 direction = newPoint - previousPoint;
            float distance = Vector3.Distance(previousPoint, newPoint);
            

            if (Physics.Raycast(previousPoint, direction, distance, ~playerMask))
            { break; }

            linePoints.Add(newPoint);            
        }

        line.positionCount = Mathf.Max(0, linePoints.Count);
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
