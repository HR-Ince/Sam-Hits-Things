using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour
{
    [SerializeField] float maximumDescent;

    private bool liftLeft;
    private Vector3 startPos;
    private Vector3 targetPos;

    private void Awake()
    {
        startPos = transform.parent.position; 
        targetPos = new Vector3(transform.position.x, transform.position.y - maximumDescent, transform.position.z);
    }
    private void Update()
    {
        if (liftLeft)
            Return();
    }
    public void Descend()
    {
        liftLeft = false;
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPos, Time.deltaTime);
    }
    public void MarkLeft()
    {
        liftLeft = true;
    }
    private void Return()
    {
        if(transform.parent.position == startPos) { return; }
        else
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, startPos, Time.deltaTime);
        }
        
    }

}
