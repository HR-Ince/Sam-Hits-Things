using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ContinueButtonHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Color inactiveColor = Color.grey;
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] WorldState worldState;

    private bool isActive;

    private void OnEnable()
    {
        TMP_Text text = GetComponent<TMP_Text>();

        if (worldState.LastLevelPlayed <= 1)
            isActive = false;
        else
        {
            isActive = true;
        }

        if (isActive)
            text.color = activeColor;
        else
            text.color = inactiveColor;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (!isActive) { return; }
        
        SceneManager.LoadScene(worldState.LastLevelPlayed);
    }
}
