using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent response;

    private void OnEnable()
    { gameEvent.AddListener(this); }
    private void OnDisable()
    { gameEvent.RemoveListener(this); }
    public void Fire()
    {
        if (response != null)
        {
            print("Fire GEL");
            response.Invoke();
        }
            
    }
}
