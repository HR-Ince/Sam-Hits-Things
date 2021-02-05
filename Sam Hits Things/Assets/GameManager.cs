using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

        DontDestroyOnLoad(this);
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        print("Scene: " + scene.buildIndex);
        worldState.LastLevelPlayed = scene.buildIndex;
        worldState.InGame = true;
    }

}
