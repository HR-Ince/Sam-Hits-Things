using UnityEngine;
using UnityEngine.UI;

public class PlayerTargettingManager : MonoBehaviour
{
    [SerializeField] float maxDraw = 10f, minDrawX, minDrawY, minimumDrawPercentage = 0.2f;
    [SerializeField] Image debugPress;

    public Vector3 DirectionVector { get { return dirVector; } }
    public float DrawPercentage { get { return drawPercentage; } }

    private Vector3 dirVector;
    private float drawPercentage;
    private Vector3 pressPos;

    public void SetupTargetting(Vector3 inputPos)
    {
        ResetValues();
        SetPressPos(inputPos);
    }
    private void SetPressPos(Vector3 inputPos)
    {
        pressPos = inputPos;
        debugPress.transform.position = inputPos;
    }
    public void AdjustTargetting(Vector3 inputPos)
    {
        float xDraw = (pressPos.x - inputPos.x) / 40; // Division to "convert" from screen units
        xDraw = Mathf.Clamp(xDraw, minDrawX, maxDraw);
        float yDraw = (pressPos.y - inputPos.y) / 20; // Division to "convert" from screen units
        yDraw = Mathf.Clamp(yDraw, minDrawY, maxDraw);
        Vector3 drawVector = new Vector3(xDraw, yDraw);

        float drawLength = Vector2.Distance(Vector2.zero, drawVector);

        drawPercentage = Mathf.Clamp(drawLength / maxDraw, 0, 1);

        dirVector = Vector3.Normalize(drawVector);
    }
    public bool DrawIsSufficient()
    {
        return drawPercentage > minimumDrawPercentage;
    }
    private void ResetValues()
    {
        dirVector = Vector3.zero;
        drawPercentage = 0.0f;
    }
}
