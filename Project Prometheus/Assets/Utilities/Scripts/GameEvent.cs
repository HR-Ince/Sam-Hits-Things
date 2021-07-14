using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> listeners = new List<GameEventListener>();

    public void AddListener(GameEventListener gel)
    { listeners.Add(gel); }
    public void RemoveListener(GameEventListener gel)
    { listeners.Remove(gel); }
    public void Invoke()
    {
        if (listeners.Count <= 0)
            return;

        for(int i = listeners.Count - 1; i >= 0; i--)
        { listeners[i].Fire(); }
    }
}
