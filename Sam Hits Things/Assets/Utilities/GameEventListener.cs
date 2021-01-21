using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent response;

    private void Awake()
    {
        gameEvent.AddListener(this);
    }
    public void Fire()
    {
        if (response != null)
            response.Invoke();
    }
}
