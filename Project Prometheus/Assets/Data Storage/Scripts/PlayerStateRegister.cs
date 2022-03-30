using UnityEngine;

[CreateAssetMenu]
public class PlayerStateRegister : ScriptableObject
{
    public bool IsActiveVessel { get; set; }
    public GameObject PlayerOne { get { return playerOne; } set { playerOne = value; } }
    public Vector3 SpawnPoint { get { return spawnPoint; } set { spawnPoint = value; } }

    private GameObject playerOne = null;
    private Vector3 spawnPoint;
    
}
