using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonHandler : MonoBehaviour
{
    [SerializeField] WorldState worldState;
    [SerializeField] TMP_Text buttonText;

    private void OnEnable()
    {
        if (worldState.LastLevelPlayed <= 1)
            buttonText.text = "Start";
        else
            buttonText.text = "Continue";
    }
    public void LoadLevel()
    {
        if (worldState.LastLevelPlayed <= 1)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(worldState.LastLevelPlayed);

    }
}
