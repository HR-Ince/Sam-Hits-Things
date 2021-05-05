using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargettingDotUI : MonoBehaviour
{
    [SerializeField] PlayerSettings settings;
    [SerializeField] PlayerStateRegister register;
    [SerializeField] Image targettingDot;
    [SerializeField] TMP_Text launchStrengthText;

    private ManualLauncher targeter;
    
    private void Awake()
    {
        targeter = GetComponent<ManualLauncher>();
        targettingDot.color = settings.TargettingDotColor;
        targettingDot.enabled = false;
    }

    private void Update()
    {
        if(targeter.DrawPercentage > 0)
        {
            targettingDot.enabled = true;
            targettingDot.gameObject.transform.position = Camera.main.WorldToScreenPoint(targeter.TargetVector);
        }
        else
        {
            targettingDot.enabled = false;
        }

        launchStrengthText.text = targeter.DrawPercentage.ToString("F2");
    }
}
