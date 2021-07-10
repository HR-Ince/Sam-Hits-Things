using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] float delayBeforeCollect = 2;
    [SerializeField] GameObject heldObject;
    [SerializeField] Transform holdPosTransform;
    [SerializeField] PlayerStateRegister register;
    [SerializeField] bool collectionNeedsPower;

    private bool isHolding;
    private bool isMovingToCollect = false;
    private float baseYPos;
    private float timeHeldObjLost;
    private Vector3 holdPosObjOffset;

    private PointToPointMovement pointToPoint;
    private Rigidbody heldObjRB;

    private void Awake()
    {
        if(heldObject == null) { Debug.LogError("Held object missing from " + gameObject.name); }
        else { heldObjRB = heldObject.GetComponent<Rigidbody>(); }
        if (holdPosTransform == null) { Debug.LogError("Hold position transform missing from " + gameObject.name); }
        pointToPoint = GetComponent<PointToPointMovement>();
        if(pointToPoint == null) { Debug.LogError("Point to point movement missing from " + gameObject.name); }
        holdPosObjOffset = holdPosTransform.position - transform.position;

        baseYPos = transform.position.y;
        
        Rigidbody[] children = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody child in children)
        {
            if (child.gameObject == heldObject)
                isHolding = true;
            else
                isHolding = false;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(holdPosTransform.position, heldObject.transform.position) < 0.001f && isMovingToCollect)
        {/*
            if(heldObject.transform.parent != null)
            {
                if(heldObject.transform.parent.TryGetComponent(out PlayerGrabHandler player))
                {
                    player.ReleaseHeldObject();
                }
            }*/
            heldObject.transform.parent = transform;
            heldObjRB.isKinematic = true;
            isMovingToCollect = false;
            isHolding = true;
        }
        if (!isHolding)
        {
            if(Time.time - timeHeldObjLost < delayBeforeCollect) { return; }
            FindPointsForCollection();
            Activate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == register.PlayerOne && isHolding)
        {
            isHolding = false;
            heldObject.transform.parent = null;
            heldObjRB.isKinematic = false;
            timeHeldObjLost = Time.time;
        }
    }
    public void Activate()
    {
        if (!isHolding && !isMovingToCollect)
        {
            CollectHeldObject();
        }
    }
    private void FindPointsForCollection()
    {
        Vector3 heldObjX = new Vector3(heldObject.transform.position.x, transform.position.y);
        print("Actual pos: " + heldObject.transform.position.x + ", read pos: " + heldObjX.x);
        Vector3 heldObjCollectableY = heldObject.transform.position - holdPosObjOffset;
        Vector3 startingPosY = new Vector3(heldObject.transform.position.x, baseYPos);
        Vector3[] pointsForCollection = new Vector3[] { heldObjX, heldObjCollectableY, startingPosY };
        print("Passed pos: " + pointsForCollection[1].x);
        pointToPoint.OverridePoints(pointsForCollection);
    }
    private void CollectHeldObject()
    {
        pointToPoint.Override(collectionNeedsPower);

        isMovingToCollect = true;
    }
}
