using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [SerializeField] int[] launchTargets = new int[3];

    [SerializeField] GameObject statContainer;
    [SerializeField] WorldStats worldStats;

    [Header("Text elements")]
    [SerializeField] TMP_Text launchRanking;
    [SerializeField] TMP_Text slayerRanking;

    [HideInInspector] public GameObject StatContainer { get { return statContainer; } }

    private int[] enemyDefeatedTargets = new int[3];
    private int totalLaunches;

    private void Awake()
    {
        for(int i = 0; i < launchTargets.Length; i++)
        {
            if (launchTargets[i] == 0)
                Debug.LogError("Launch targets cannot have a value of zero");
        }

        int totalEnemies = worldStats.EnemiesInLevel;
        enemyDefeatedTargets[0] = totalEnemies;
        if(totalEnemies > 0)
        {
            enemyDefeatedTargets[1] = Mathf.RoundToInt(totalEnemies * 0.75f);
            enemyDefeatedTargets[2] = Mathf.RoundToInt(totalEnemies * 0.5f);
        }

        totalLaunches = 0;
    }

    public void AddJump()
    {
        totalLaunches++;
    }
    public void ExitSummary()
    {
        statContainer.SetActive(true);

        if (totalLaunches <= launchTargets[0])
            launchRanking.text = "Gold";
        else if (totalLaunches <= launchTargets[1])
            launchRanking.text = "Silver";
        else if (totalLaunches <= launchTargets[2])
            launchRanking.text = "Bronze";
        else
            launchRanking.text = "Participation Award";

        int enemiesDefeated = enemyDefeatedTargets[0] - worldStats.EnemiesInLevel;

        if (enemiesDefeated == enemyDefeatedTargets[0])
            slayerRanking.text = "Gold";
        else if (enemiesDefeated >= enemyDefeatedTargets[1])
            slayerRanking.text = "Silver";
        else if (enemiesDefeated >= enemyDefeatedTargets[2])
            slayerRanking.text = "Bronze";
        else
            slayerRanking.text = "Participation Award";

        worldStats.Reset();
    }
}
