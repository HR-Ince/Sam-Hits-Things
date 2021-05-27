using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    [SerializeField] int playerLayer, enemyLayer, wallLayer, floorLayer, foregroundLayer, interactableLayer, shieldingLayer;

    public int PlayerLayer { get { return playerLayer; } }
    public int PlayerLayerMask { get { return ConvertToMask(playerLayer); } }

    public int EnemyLayer { get { return enemyLayer; } }
    public int EnemyLayerMask { get { return ConvertToMask(enemyLayer); } }

    public int WallLayer { get { return wallLayer; } }
    public int WallLayerMask { get { return ConvertToMask(wallLayer); } }

    public int FloorLayer { get { return floorLayer; } }
    public int FloorLayerMask { get { return ConvertToMask(floorLayer); } }

    public int ForegroundLayer { get { return foregroundLayer; } }
    public int ForegroundLayerMask { get { return ConvertToMask(foregroundLayer); } }

    public int InteractableLayer { get { return interactableLayer; } }
    public int InteractableLayerMask { get { return ConvertToMask(interactableLayer); } }

    public int ShieldingLayer { get { return shieldingLayer; } }
    public int ShieldingLayerMask { get { return ConvertToMask(shieldingLayer); } }

    private int ConvertToMask(int layer)
    {
        return 1 << layer;
    }
}
