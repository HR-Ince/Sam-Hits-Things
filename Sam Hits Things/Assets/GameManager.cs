using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerStateRegister register;
    [SerializeField] WorldState worldState;

    private void Awake()
    {
        GameManager[] gMs = FindObjectsOfType<GameManager>();
        if (gMs.Length > 1)
            Destroy(this);
        else
            DontDestroyOnLoad(this);
    }
    public void ResetLevel()
    {
        //register.PlayerOne.transform.position = register.SpawnPoint;
        PositionResetHandler[] reseters = FindObjectsOfType<PositionResetHandler>();

        foreach(PositionResetHandler reseter in reseters)
        {
            reseter.ResetPosition();
        }
    }

}
