using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MouseData : ScriptableObject
{
    [SerializeField] Vector2 targetPos;
    [SerializeField] Vector2 directionalVector;
    [SerializeField] float drawPercentage;

    public Vector2 TargetPos { get { return targetPos; } set { targetPos = value; } }
    public Vector2 DirectionVector { get { return directionalVector; } set { directionalVector = value; } }
    public float DrawPercentage { get { return drawPercentage; } set { drawPercentage = value; } }

    private void Awake()
    {
        ResetValues();
    }
    public void ResetValues()
    {
        targetPos = Vector2.zero;
        directionalVector = Vector2.zero;
        drawPercentage = 0f;
    }
}
