using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] Color baseColor = Color.black;
    [SerializeField] Color maxedColor = Color.red;

    [SerializeField] MouseData mouseData;
    [SerializeField] PlayerRegister register;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (!line) { Debug.LogError("Line Renderer missing from LineDrawer Gameobject"); return; }
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
    public void SetLinePositions()
    {
        if(mouseData.DrawPercentage <= 0.0f) { return; }

        Vector3 lineEnd = mouseData.TargetPos;

        line.SetPosition(1, lineEnd);
    }
}
