using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data Storage/Active Demon Holder")]
public class ActiveObjects : ScriptableObject
{
    public GameObject ActiveDemon { get { return activeDemon; } }
    public void SetActiveDemon(GameObject demon) { activeDemon = demon; }
    private GameObject activeDemon;

    public ContextMenuManager ActiveMenu { get { return activeMenu; } }
    public void SetActiveMenu(ContextMenuManager menu) { activeMenu = menu; }
    private ContextMenuManager activeMenu;

    public ShrineController ActiveShrine { get { return activeShrine; } }
    public void SetActiveShrine(ShrineController shrine) { activeShrine = shrine; }
    private ShrineController activeShrine;

    private void OnEnable()
    {
        activeShrine = null;
        activeDemon = null;
        activeMenu = null;
    }
}
