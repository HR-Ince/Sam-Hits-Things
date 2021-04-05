using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour
{
    [SerializeField] float maximumDescent;

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Awake()
    {
        startPos = transform.parent.position; 
        targetPos = new Vector3(transform.position.x, transform.position.y - maximumDescent, transform.position.z);
    }
    public void Descend()
    {
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPos, Time.deltaTime);
    }

    public void Return()
    {
        float period = 0;
        while(period != 5f)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, startPos, Time.deltaTime);
            period += Time.deltaTime;
        }
        
    }

}
