using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class ManualLauncher : MonoBehaviour
{
    [SerializeField] float thrustModifier;
    [SerializeField] float maxMagnitude = 10f, minimumDrawPercentage = 5f;

    public Vector3 DirectionVector { get { return dirVector; } }
    public Vector3 TargetVector { get { return targetVector; } }
    public float DrawPercentage { get { return drawPercentage; } }
    public float ThrustModifier { get { return thrustModifier; } }

    private float cameraDepth;
    private float drawPercentage;
    private GameObject objectToLaunch;
    private Vector3 dirVector;
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
    public void SetObjectToLaunch(GameObject obj)
    {
        objectToLaunch = obj;
    }
    public void SetPressPos(Vector3 inputPos)
    {
        pressedPosWorld = cam.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, cameraDepth));
    }
    public void AdjustTargetting(Vector3 inputPos)
    {
        Vector3 inputPosWorld = cam.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, cameraDepth));
        Vector3 posDif = Vector3.ClampMagnitude(pressedPosWorld - inputPosWorld, maxMagnitude);
        targetVector = objectToLaunch.transform.position + posDif;
        targetVector = new Vector3(targetVector.x, Mathf.Max(objectToLaunch.transform.position.y, targetVector.y));

        float difMagnitude = Mathf.Abs(Mathf.Clamp(Vector3.Distance(pressedPosWorld, inputPosWorld), 0, maxMagnitude));
        dirVector = posDif / difMagnitude;
        drawPercentage = Mathf.Clamp(difMagnitude / maxMagnitude * 100, 0, 100);
    }
    public bool WillLaunch()
    {
        return drawPercentage > minimumDrawPercentage;
    }
    public void Launch()
    {
        float thrust = drawPercentage * thrustModifier;
        objectToLaunch.GetComponent<Rigidbody>().AddForce(dirVector * thrust);
        ResetValues();
    }
    private void ResetValues()
    {
        targetVector = objectToLaunch.transform.position;
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
