using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] InfusionButtons;

    // Scriptable references
    [SerializeField] PlayerStateRegister _playerRegister;


    private void Update()
    {
        SetInfusionsActive(!_playerRegister.IsActiveVessel);
    }

    public void SetInfusionsActive(bool value)
    {
        if (InfusionButtons[0].activeSelf == value) return;

        foreach (var button in InfusionButtons)
        {
            button.SetActive(value);
        }
    }
}
