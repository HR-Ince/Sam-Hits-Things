using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldState : ScriptableObject
{
    [SerializeField] private int floorLayer, wallLayer, foregroundLayer, interactableLayer;

    public int WallLayer { get { return wallLayer; } }
    public int FloorLayer { get { return floorLayer; } }
    public int ForegroundLayer { get { return foregroundLayer; } }
    public int InteractableLayer { get { return interactableLayer; } }

    [Min(1)] int lastLevelPlayed;
    public int LastLevelPlayed { get { return lastLevelPlayed; } 
        set 
        {
            if(value < 1) { return; }
            lastLevelPlayed = value; 
        } }
    public bool InGame { get; set; } = true;
}
