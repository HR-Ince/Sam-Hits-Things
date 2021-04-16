using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldState : ScriptableObject
{
    [SerializeField] private int floorLayer, wallLayer, foregroundLayer;

    public int WallLayer { get { return wallLayer; } }
    public int FloorLayer { get { return floorLayer; } }
    public int ForegroundLayer { get { return foregroundLayer; } }

    [Min(1)] int lastLevelPlayed;
    public int LastLevelPlayed { get { return lastLevelPlayed; } 
        set 
        {
            if(value < 1) { return; }
            lastLevelPlayed = value; 
        } }
    public bool InGame { get; set; } = true;
}
