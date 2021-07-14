using UnityEngine;

[CreateAssetMenu]
public class WorldStats : ScriptableObject
{
    [SerializeField] int launchesMade;
    [SerializeField] int enemiesInLevel;
    public int LaunchesMade { get { return launchesMade; } set { launchesMade = value; } }
    public int EnemiesInLevel { get { return enemiesInLevel; } set { enemiesInLevel = value; } }

    private void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        launchesMade = 0;
        enemiesInLevel = 0;
    }
}
