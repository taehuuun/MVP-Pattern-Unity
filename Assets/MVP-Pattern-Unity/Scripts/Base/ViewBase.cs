using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ViewBase<T> : MonoBehaviour, IView<T>
{
    private readonly Dictionary<string, UnityEventBase> _events = new();

    protected abstract void Initialize();
    
    public abstract void UpdateView(T data);
}
