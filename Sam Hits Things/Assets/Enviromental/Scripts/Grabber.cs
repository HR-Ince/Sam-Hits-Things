using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] GameObject heldObject;
    [SerializeField] Transform holdPosTransform;
    [SerializeField] PlayerRegister register;
    [SerializeField] bool movementOverrideRequiresPower;

    private bool isHolding;
    private bool isMovingToCollect = false;
    private float baseYPos;
    private Vector3 holdPosObjOffset;

    private PointToPointMovement pointToPoint;

    private void Awake()
    {
        if(heldObject == null) { Debug.LogError("Held object missing from " + gameObject.name); }
        if (holdPosTransform == null) { Debug.LogError("Hold position transform missing from " + gameObject.name); }
        pointToPoint = GetComponent<PointToPointMovement>();
        if(pointToPoint == null) { Debug.LogError("Point to point movement missing from " + gameObject.name); }
        holdPosObjOffset = holdPosTransform.position - transform.position;

        baseYPos = transform.position.y;

        if (heldObject.transform.position != holdPosTransform.position)
            isHolding = false;
        else
            isHolding = true;
    }

    private void Update()
    {
        if(Vector3.Distance(holdPosTransform.position, heldObject.transform.position) < 0.001f)
        {
            isHolding = true;
        }
        if(isHolding)
            heldObject.transform.position = holdPosTransform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == register.PlayerOne && isHolding)
        {
            isHolding = false;
            isMovingToCollect = false;
        }
    }
    public void Activate()
    {
        if (!isHolding && !isMovingToCollect)
        {
            CollectHeldObject();
        }
    }
    private void CollectHeldObject()
    {
        Vector3 heldObjX = new Vector3(heldObject.transform.position.x, transform.position.y);
        Vector3 heldObjCollectableY = heldObject.transform.position - holdPosObjOffset;
        Vector3 startingPosY = new Vector3(heldObject.transform.position.x, baseYPos);
        Vector3[] pointsToPass = new Vector3[] { heldObjX, heldObjCollectableY, startingPosY };

        pointToPoint.Override(pointsToPass, movementOverrideRequiresPower);

        isMovingToCollect = true;
    }
}
