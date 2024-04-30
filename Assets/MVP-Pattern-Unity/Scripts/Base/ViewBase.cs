using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ViewBase<T> : MonoBehaviour, IView<T>
{
    private readonly Dictionary<string, UnityEventBase> _events = new();

    public abstract void UpdateView(T data);
}
