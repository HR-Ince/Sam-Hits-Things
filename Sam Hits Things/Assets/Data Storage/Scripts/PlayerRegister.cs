using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerRegister : ScriptableObject
{
    private GameObject playerOne = null;
    public GameObject PlayerOne { get { return playerOne; } set { playerOne = value; } }
    private Vector3 spawnPoint;
    public Vector3 SpawnPoint { get { return spawnPoint; } set { spawnPoint = value; } }
}
