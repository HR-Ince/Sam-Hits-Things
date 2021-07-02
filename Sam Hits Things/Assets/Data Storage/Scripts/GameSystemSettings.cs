using UnityEngine;

[CreateAssetMenu(fileName = "Game settings", menuName = "Settings/Game Settings")]
public class GameSystemSettings : ScriptableObject
{
    private bool doesAutomaticallySwitchDirection;
    private bool doesFireOnRelease;
    private bool doesDisplayScorecard;
    private int percentageOfTrajectoryShown;

    public bool DoesAutomaticallySwitchDirection { get { return DoesAutomaticallySwitchDirection; } }
    public bool DoesFireOnRelease { get { return doesFireOnRelease; } }
    public bool DoesDisplayScorecard { get { return doesDisplayScorecard; } }
    public int PercentageOfTrajectoryShown { get { return percentageOfTrajectoryShown; } }

    public void RestoreDefaults()
    {
        doesAutomaticallySwitchDirection = true;
        doesFireOnRelease = true;
        doesDisplayScorecard = true;
        percentageOfTrajectoryShown = 10;
    }
    public void SetAutomaticallySwitchDirection(bool value)
    {
        doesAutomaticallySwitchDirection = value;
    }

    public void SetFireOnRelease(bool value)
    {
        doesFireOnRelease = value;
    }

    public void SetDisplayScorecard(bool value)
    {
        doesDisplayScorecard = value;
    }

    public void SetPercentageOfTrajectoryShown(int value)
    {
        percentageOfTrajectoryShown = value;
    }
}
