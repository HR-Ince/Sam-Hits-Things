using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] Color baseColor = Color.black;
    [SerializeField] Color maxedColor = Color.red;

    [SerializeField] PlayerRegister register;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;

        line.startColor = baseColor;
    }
    public void SetFirstPointToPlayer()
    {
        line.SetPosition(0, register.PlayerOne.transform.position);
        line.SetPosition(1, line.GetPosition(0));
    }
    public void SetLinePositions(Vector3 targetVector)
    {
        //if(mouseData.DrawPercentage <= 0.0f) { return; }

        line.SetPosition(1, targetVector);
    }
}
