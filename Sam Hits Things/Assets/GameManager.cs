using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CallerLoc
{
    left,
    right,
    up,
    down
}

public class GameManager : MonoBehaviour
{
    [SerializeField] StatHandler statHandler;
    [SerializeField] PlayerRegister register;
    [SerializeField] int worldGridWidth, worldGridHeight;
    [SerializeField] int firstRoomInBuildIndex;
    [SerializeField] WorldState worldState;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
    private void Awake()
    {
        GameManager[] gMs = FindObjectsOfType<GameManager>();
        if (gMs.Length > 1)
            Destroy(this);
        else
            DontDestroyOnLoad(this);
    }
    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1) { return; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        worldState.LastLevelPlayed = scene.buildIndex;
        worldState.InGame = true;
    }
    public void ResetLevel()
    {
        worldState.InGame = true;
        statHandler.StatContainer.SetActive(false);
        register.PlayerOne.transform.position = register.SpawnPoint;
    }

}
