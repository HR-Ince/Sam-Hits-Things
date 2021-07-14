using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] Color targettingDotColor;
    private Material playerSkin;
    public Color TargettingDotColor { get { return targettingDotColor; } set { targettingDotColor = value; } }
    public Material PlayerSkin { get { return playerSkin; } set { playerSkin = value; } }
}
