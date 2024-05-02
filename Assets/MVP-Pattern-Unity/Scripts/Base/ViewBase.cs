using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ViewBase<TModel> : MonoBehaviour, IView<TModel> where TModel : ModelBase
{
    private readonly Dictionary<string, UnityEventBase> _events = new();

    private void Start()
    {
        Initialize();
    }

    protected abstract void Initialize();
    
    public abstract void UpdateView(TModel model);

    public virtual void AddListener(string eventName, UnityAction listener)
    {
        if (_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent());
        }

        ((UnityEvent)_events[eventName]).AddListener(listener);
    }

    public virtual void AddListener<T>(string eventName, UnityAction<T> listener)
    {
        if (_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent<T>());
        }

        ((UnityEvent<T>)_events[eventName]).AddListener(listener);
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

    public virtual void RemoveListener<T>(string eventName, UnityAction<T> listener)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<T> unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }

    public virtual void RemoveEvent(string eventName)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            unityEventBase.RemoveAllListeners();
        }
        
        _events.Remove(eventName);
    }

    public virtual void RemoveAllEvent()
    {
        foreach (var events in _events.Values)
        {
            events.RemoveAllListeners();
        }

        _events.Clear();
    }

    public virtual void TriggerEvent(string eventName)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.Invoke();
            }
        }
    }
}
