using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] InfusionButtons;

    // Scriptable references
    [SerializeField] PlayerStateRegister _playerRegister;


    private void Update()
    {
        SetActiveInfusions(!_playerRegister.IsActiveVessel);
    }

    public void SetActiveInfusions(bool value)
    {
        if (InfusionButtons[0].activeSelf == value) return;

        foreach (var button in InfusionButtons)
        {
            button.SetActive(value);
        }
    }
}
