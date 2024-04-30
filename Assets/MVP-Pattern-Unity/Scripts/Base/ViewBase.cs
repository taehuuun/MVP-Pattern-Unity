using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ViewBase<T> : MonoBehaviour, IView<T>
{
    private readonly Dictionary<string, UnityEventBase> _events = new();

    private void Start()
    {
        Initialize();
    }

    protected abstract void Initialize();
    
    public abstract void UpdateView(T data);

    public virtual void AddListener(string eventName, UnityAction listener)
    {
        if (_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent());
        }

        ((UnityEvent)_events[eventName]).AddListener(listener);
    }

    public virtual void AddListener<TU>(string eventName, UnityAction<TU> listener)
    {
        if (_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent<TU>());
        }

        ((UnityEvent<TU>)_events[eventName]).AddListener(listener);
    }

    public virtual void RemoveListener(string eventName, UnityAction listener)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }
}
