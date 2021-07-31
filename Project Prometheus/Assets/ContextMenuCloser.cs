using UnityEngine;

public class ContextMenuCloser : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    private ContextMenuManager[] menues;

    private void Update()
    {
        if (input.Pressed)
        {
            menues = FindObjectsOfType<ContextMenuManager>();
            if (menues.Length > 0)
            {
                foreach(ContextMenuManager menu in menues)
                    menu.DeactivateMenu();
            }
                
        }
    }
}
