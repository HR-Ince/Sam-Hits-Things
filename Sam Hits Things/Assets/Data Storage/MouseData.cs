using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MouseData : ScriptableObject
{
    public Vector2 ClickPos { get; set; }
    public float DragLength { get; set; }
}
