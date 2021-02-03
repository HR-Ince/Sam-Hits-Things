using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] float lineWidth = 1f;
    [SerializeField] Color baseColor = Color.black;
    [SerializeField] Color maxedColor = Color.red;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;

        line.startColor = baseColor;
    }
    public void SetFirstPoint(Vector3 pos)
    {
        line.SetPosition(0, pos);
    }
    public void SetLinePositions(int index, Vector3 targetVector)
    {
        line.enabled = true;
        line.SetPosition(index, targetVector);
    }
    public void DisableLine()
    {
        line.enabled = false;
    }
}
