using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    [SerializeField] bool isManuallyActivated;
    [SerializeField] bool doesRepeatPathAutomatically;
    [SerializeField] Transform[] points;
    [SerializeField] float speed = 6;
    [SerializeField] float movementDelay = 2;

    private bool overrideOn;
    private bool manualCache;
    private Vector3[] currentPoints;
    private float startTime;
    private float tolerance;
    private int currentPointIndex;
    private Vector3 currentTarget;

    private void Start()
    {
        currentPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            currentPoints[i] = points[i].position;
        }

        if (currentPoints.Length > 0)
        {
            currentTarget = currentPoints[0];
            currentPointIndex = 0;
        }
        
        tolerance = speed * Time.deltaTime;
        manualCache = isManuallyActivated;
    }

    private void Update()
    {
        if (!isManuallyActivated)
            MoveBetweenPoints();        
    }
    public void Override(Vector3[] pointsPassed, bool isPowerDependant)
    {
        currentPoints = pointsPassed;
        overrideOn = true;
        currentTarget = currentPoints[0];
        currentPointIndex = 0;
        isManuallyActivated = isPowerDependant;
    }
    public void ResetAsOverride()
    {
        Vector3[] resetPoints = new Vector3[points.Length];
        for (int i = points.Length - 1; i >= 0; i--)
        {
            for(int j = 0; j < points.Length; j++)
            {
                resetPoints[j] = points[i].position;
            }
        }
        Override(resetPoints, false);
    }
    public void TurnOverrideOff()
    {
        for(int i = 0; i < points.Length; i++)
        {
            currentPoints[i] = points[i].position;
        }
        overrideOn = false;
        isManuallyActivated = manualCache;
    }
    public void ManualActivation()
    {
        if (!isManuallyActivated) { return; }
        if(startTime == 0)
            startTime = Time.time;
        MoveBetweenPoints();
    }
    private void MoveBetweenPoints()
    {
        if(currentPoints.Length <= 0) { Debug.LogError("No points in PointToPointMovement of " + gameObject.name); return; }

        if (transform.position != currentTarget)
        {
            Move();
        }
        else
        {
            UpdateTarget();
        }
    }

    private void Move()
    {
        if(Time.time - startTime < movementDelay) { return; }
        Vector3 heading = currentTarget - transform.position;
        transform.position += heading.normalized * speed * Time.deltaTime;
        if(heading.magnitude < tolerance)
        {
            transform.position = currentTarget;
        }
    }
    public void UpdateTarget()
    {
        if(currentPointIndex < currentPoints.Length - 1)
        {
            currentPointIndex++;
        }
        else if(currentPointIndex == currentPoints.Length - 1 && overrideOn)
        {
            TurnOverrideOff();
        }
        else if(doesRepeatPathAutomatically)
        {
            currentPointIndex = 0;
        }
        else
        {
            return;
        }
        startTime = 0;
        currentTarget = currentPoints[currentPointIndex];
    }
}
