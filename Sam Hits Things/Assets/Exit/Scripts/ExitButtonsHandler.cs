using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonsHandler : MonoBehaviour
{
    [SerializeField] WorldState worldState;
    [SerializeField] PlayerRegister register;
    private StatHandler statHandler;

    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
    }
    public void ResetLevel()
    {
        worldState.InGame = true;
        statHandler.StatContainer.SetActive(false);
        register.PlayerOne.transform.position = register.SpawnPoint;
    }
    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings) { return; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
