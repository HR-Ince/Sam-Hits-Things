using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickData : MonoBehaviour
{
    [SerializeField] float targetMaximumExtension = 10f;

    [SerializeField] GameObject posKnob, targetKnob, worldKnob;

    [SerializeField] LayerMask targetableLayer;
    [SerializeField] MouseData mouseData;
    [SerializeField] PlayerRegister register;

    private Vector3 playerPos;
    private Vector3 pressedPosScreen;
    private Vector3 pressedPosWorld;

    public void SetPressPos()
    { 
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
        {
            pressedPosScreen = Input.mousePosition;
            pressedPosWorld = hit.point;
            posKnob.transform.position = hit.point;
        }
    }
    public void SetPlayerPos()
    {
        playerPos = register.PlayerOne.transform.position;
    }
    public void BeginTargeting()
    {
        Vector3 worldTarget = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetableLayer))
            worldTarget = hit.point;

        AdjustTargetting(playerPos, worldTarget);
    }
    private void AdjustTargetting(Vector3 originPos, Vector3 inputPos)
    {
        Vector3 posDif = Vector3.ClampMagnitude(new Vector3(pressedPosWorld.x - inputPos.x, pressedPosWorld.y - inputPos.y), targetMaximumExtension);
        Vector2 targetVector = originPos + posDif;
        Vector3 aimVector = pressedPosWorld + posDif;

        Vector3 dirVector = posDif / Vector3.Distance(originPos, targetVector);
        
        mouseData.DirectionVector = dirVector;
        mouseData.TargetPos = targetVector;
        targetKnob.transform.position = aimVector;
        worldKnob.transform.position = targetVector;
        
    }
    public void GetDragLength()
    {
        Vector3 inputPos = Input.mousePosition;

        float magnitiude = Vector3.Distance(pressedPosScreen, inputPos);
        float magAsPercent = Mathf.Clamp(magnitiude / (targetMaximumExtension * Screen.dpi) * 100, 0, 100);

        mouseData.DrawPercentage = magAsPercent;
    }
    public void ResetValues()
    {
        mouseData.ResetValues();
    }
}
