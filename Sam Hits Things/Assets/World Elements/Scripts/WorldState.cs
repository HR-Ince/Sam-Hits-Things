using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldState : ScriptableObject
{
    [SerializeField] private float floorLayer, wallLayer;

    public float WallLayer { get { return wallLayer; } }
    public float FloorLayer { get { return floorLayer; } }

    [Min(1)] int lastLevelPlayed;
    public int LastLevelPlayed { get { return lastLevelPlayed; } 
        set 
        {
            if(value < 1) { return; }
            lastLevelPlayed = value; 
        } }
    public bool InGame { get; set; } = true;
}
