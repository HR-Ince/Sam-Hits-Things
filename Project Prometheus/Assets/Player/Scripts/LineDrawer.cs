using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    //Private variables
    [SerializeField] float _lineWidth = 1f;
    [SerializeField] int _linePointsPerUnit = 10;
    [SerializeField] LayerMask _playerMask;

    private List<Vector3> LinePoints = new List<Vector3>();


    // Component references
    private PlayerDrawHandler _targeter;
    private LineRenderer _line;
    

    private void Awake()
    {
        FetchExternalVariables();
        SetLineVariables();
    }
    private void FetchExternalVariables()
    {
        _line = GetComponent<LineRenderer>();
        _targeter = GetComponent<PlayerDrawHandler>();
        if (_targeter == null) { Debug.LogError("Targeter missing from line drawer GO"); }
    }
    private void SetLineVariables()
    {
        _line.startWidth = _lineWidth;
        _line.endWidth = _lineWidth;
        _line.enabled = false;
    }

    public void ManageTrajectoryLine(Rigidbody rb, float thrust, ForceMode forceMode)
    {
        Vector3 targetDirection = _targeter.DirectionVector;

        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        _line.enabled = true;
        
        float forceDuration = forceMode == ForceMode.Force || forceMode == ForceMode.Acceleration ? Time.fixedDeltaTime : 1;
        float mass = forceMode == ForceMode.Impulse || forceMode == ForceMode.Force ? rb.mass : 1;
        
        Vector3 forceVector = targetDirection * (_targeter.DrawPercentage * thrust);
        Vector3 velocityVector = (forceVector / mass) * forceDuration;
        

        float gravity = -Physics.gravity.y;
        float flightTime = Vector3.Magnitude(velocityVector) / (gravity / 2);
        float linePositions = Mathf.FloorToInt(flightTime * _linePointsPerUnit);
        float stepInterval = flightTime / linePositions;

        LinePoints.Clear();
        
        float stepTimePassed;
        Vector3 startingPos = rb.transform.position;

        for (int i = 0; i < linePositions; i++)
        {
            stepTimePassed = stepInterval * i;

            float xPos = startingPos.x + velocityVector.x * stepTimePassed;
            float yPos = startingPos.y + velocityVector.y * stepTimePassed - 0.5f * gravity * stepTimePassed * stepTimePassed;
            Vector3 newPoint = new Vector3(xPos, yPos, 0);

            if (i == 0) 
            {
                LinePoints.Add(newPoint);
                continue;
            }

            Vector3 previousPoint = LinePoints[i - 1];

            Vector3 direction = newPoint - previousPoint;
            float distance = Vector3.Distance(previousPoint, newPoint);
            
            // Check line doesn't pass through objects
            if (Physics.Raycast(previousPoint, direction, distance, ~_playerMask))
            { break; }

            LinePoints.Add(newPoint);            
        }

        _line.positionCount = Mathf.Max(0, LinePoints.Count);
        _line.SetPositions(LinePoints.ToArray());
    }
    public void ManageBasicLine(Vector3 targetDirection, float length)
    {
        if (float.IsNaN(targetDirection.x) || float.IsNaN(targetDirection.y)) { return; }
        _line.enabled = true;
        Vector3 targetVector = Vector3.ClampMagnitude(targetDirection * length, 5);
        targetVector += _line.GetPosition(0);

        for (int i = 1; i < _line.positionCount; i++)
        {
            float interval = (float)i / _line.positionCount;
            _line.SetPosition(i, Vector3.Lerp(_line.GetPosition(0), targetVector, interval));
        }
    }
    public void DisableLine()
    {
        _line.enabled = false;
    }
}
