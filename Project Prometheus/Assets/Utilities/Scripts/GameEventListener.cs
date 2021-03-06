﻿using UnityEngine;
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
            response.Invoke();
        }
            
    }
}
