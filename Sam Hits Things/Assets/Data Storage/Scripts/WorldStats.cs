﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldStats : ScriptableObject
{
    [SerializeField] int launchesMade;
    [SerializeField] int enemiesInLevel;
    public int LaunchesMade { get { return launchesMade; } set { launchesMade = value; } }
    public int EnemiesInLevel { get { return enemiesInLevel; } set { enemiesInLevel = value; } }

    private void OnDisable()
    {
        LaunchesMade = 0;
        EnemiesInLevel = 0;
    }
}