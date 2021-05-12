using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTargettingManager : MonoBehaviour
{
    [SerializeField] float maxMagnitude = 10f, minimumDrawPercentage = 5f;

    public float DrawPercentage { get { return drawPercentage; } }
    public Vector3 DirectionVector { get { return dirVector; } }
    public Vector3 TargetVector { get { return targetVector; } }

    internal Vector3 targettingOrigin;
    internal Vector3 dirVector;

    private float cameraDepth;
    private float drawPercentage;
    private Vector3 targetVector;
    private Vector3 pressedPosWorld;

    private Camera cam;

    private void Awake()
    {
        FetchExternalVariables();
    }
    private void FetchExternalVariables()
    {
        cam = Camera.main;
        cameraDepth = -Camera.main.transform.position.z;
    }
    public void SetupTargetting(Vector3 origin, Vector3 inputPos)
    {
        ResetValues();
        SetTargettingOrigin(origin);
        SetPressPos(inputPos);
    }
    private void SetTargettingOrigin(Vector3 origin)
    {
        targettingOrigin = origin;
    }
    private void SetPressPos(Vector3 inputPos)
    {
        pressedPosWorld = cam.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, cameraDepth));
    }
    public void AdjustTargetting(Vector3 inputPos)
    {
        Vector3 inputPosWorld = cam.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, cameraDepth));
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPosWorld, maxMagnitude);
        targetVector = targettingOrigin + posDif;
        targetVector = new Vector3(targetVector.x, Mathf.Max(targettingOrigin.y, targetVector.y));

        float difMagnitude = Mathf.Abs(Mathf.Clamp(Vector3.Distance(pressedPosWorld, inputPosWorld), 0, maxMagnitude));
        dirVector = posDif / difMagnitude;
        dirVector = new Vector3(dirVector.x, dirVector.y, 0);
        drawPercentage = Mathf.Clamp(difMagnitude / maxMagnitude * 100, 0, 100);
    }
    public bool DrawIsSufficient()
    {
        return drawPercentage > minimumDrawPercentage;
    }
    private void ResetValues()
    {
        targetVector = targettingOrigin;
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
