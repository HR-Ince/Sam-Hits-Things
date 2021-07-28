using UnityEngine;

[CreateAssetMenu(menuName = "Data Storage/Active Demon Holder")]
public class ActiveObjects : ScriptableObject
{
    public GameObject ActiveDemon { get { return activeDemon; } }
    public void SetActiveDemon(GameObject demon) { activeDemon = demon; }

    private GameObject activeDemon;

    public GameObject ActiveButton { get { return activeButton; } }
    public void SetActiveButton(GameObject button) { activeButton = button; }

    private GameObject activeButton;

    
}
