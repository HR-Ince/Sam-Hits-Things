using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunchHandler : MonoBehaviour
{
    // Public variables
    public Vector3 DirectionVector { get { return _dirVector; } }
    public float DrawPercentage { get { return _drawPercentage; } }

    // Private variables
    [SerializeField] float _maxDraw = 10f, _minDrawX, _minDrawY, _minimumDrawPercentage = 0.2f;

    private Vector3 _dirVector;
    private float _drawPercentage;
    private Vector3 _pressPos;

    public void SetupTargetting(Vector3 inputPos)
    {
        ResetValues();
        SetPressPos(inputPos);
    }
    private void SetPressPos(Vector3 inputPos)
    {
        _pressPos = inputPos;
    }
    public void AdjustTargetting(Vector3 inputPos)
    {
        float xDraw = (_pressPos.x - inputPos.x) / 40; // Division to "convert" from screen units
        xDraw = Mathf.Clamp(xDraw, -_maxDraw, _maxDraw);
        float yDraw = (_pressPos.y - inputPos.y) / 20; // Division to "convert" from screen units
        yDraw = Mathf.Clamp(yDraw, -_maxDraw, _maxDraw);
        Vector3 drawVector = new Vector3(xDraw, yDraw);

        float drawLength = Vector2.Distance(Vector2.zero, drawVector);

        _drawPercentage = Mathf.Clamp(drawLength / _maxDraw, 0, 1);

        _dirVector = Vector3.Normalize(drawVector);
    }
    public bool DrawIsSufficient()
    {
        return _drawPercentage > _minimumDrawPercentage;
    }
    private void ResetValues()
    {
        _dirVector = Vector3.zero;
        _drawPercentage = 0.0f;
    }
}
