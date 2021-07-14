using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonsHandler : MonoBehaviour
{
    [SerializeField] GameManager gM;
    [SerializeField] WorldState worldState;

    private void LoadNextLevel()
    {
        
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
