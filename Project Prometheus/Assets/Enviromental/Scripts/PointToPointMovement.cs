using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    private enum SwitchType { toggle, held, defaulting, deactivating }
    private enum PathType { Looping, SinglePath, SinglePoint }

    [SerializeField] SwitchType switchType;
    [SerializeField] PathType pathType;
    [SerializeField] Transform[] points;
    [SerializeField] float speed = 6;
    [SerializeField] float movementDelay = 2;

    private bool overrideOn = false;
    private bool pathReversed = false;
    private bool powerToggleOn = false;
    private bool endReached = false;
    private float startTime;
    private float tolerance;
    private int currentPointIndex;
    private PathType pathCache;
    private Vector3[] currentPoints;
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
            currentPointIndex = 0;
            currentTarget = currentPoints[currentPointIndex];
        }
        
        tolerance = speed * Time.deltaTime;
        pathCache = pathType;
        if (switchType == SwitchType.deactivating)
            powerToggleOn = true;
        else
            powerToggleOn = false;
    }
    private void FixedUpdate()
    {
        if (powerToggleOn && !endReached)
            MoveBetweenPoints();        
    }
    public void Override(bool isPowerDependant)
    {
        overrideOn = true;
        currentPointIndex = 0;
        pathType = PathType.Looping;
    }
    public void OverridePoints(Vector3[] pointsPassed)
    {
        currentPoints = pointsPassed;
        currentTarget = currentPoints[currentPointIndex];
    }
    private void EndOverride()
    {
        currentPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            currentPoints[i] = points[i].position;
        }

        currentPointIndex = 0;
        pathType = pathCache;
        overrideOn = false;
    }
    public void ManualInteraction()
    {
        if (startTime == 0)
            startTime = Time.time;

        if (switchType == SwitchType.deactivating)
        {
            powerToggleOn = false;
            return;
        }
        else
        {
            powerToggleOn = true;
            if (switchType == SwitchType.defaulting && pathReversed)
            {
                ReversePath();
                UpdateTarget();
            }
        }
    }
    public void OnPowerOff()
    {
        if(switchType == SwitchType.defaulting || switchType == SwitchType.deactivating)
        {
            powerToggleOn = true;

            if(switchType == SwitchType.defaulting && !pathReversed)
            {
                ReversePath();
            }
        }
        else if(switchType == SwitchType.held)
        {
            powerToggleOn = false;
        }

        endReached = false;
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
        if(!pathReversed && currentPointIndex != currentPoints.Length - 1 || 
            pathReversed && currentPointIndex != 0)
        {
            UpdatePointIndex();
        }
        else if(!pathReversed && currentPointIndex == currentPoints.Length - 1 ||
            pathReversed && currentPointIndex == 0)
        {
            ReversePath();
            UpdatePointIndex();
            if (pathType == PathType.SinglePath)
            {
                powerToggleOn = false;
            }

            if(pathReversed && overrideOn)
            {
                EndOverride();
            }
        }
        if(pathType == PathType.SinglePoint)
        {
            powerToggleOn = false;
            endReached = true;
        }

        startTime = 0;
        currentTarget = currentPoints[currentPointIndex];
    }
    private void UpdatePointIndex()
    {
        if (!pathReversed) { currentPointIndex++; }
        else { currentPointIndex--; }
    }
    private void ReversePath()
    {
        pathReversed = !pathReversed;
    }
}
