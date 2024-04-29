using UnityEngine;
using UnityEngine.Events;

public abstract class ModelBase<T> : MonoBehaviour, IModel<T>
{
    public T Data { get; private set; }

    private readonly UnityEvent<T> _onDataUpdated = new();
    
    public void AddListener(UnityAction<T> listener)
    {
        _onDataUpdated.AddListener(listener);
    }

    public void RemoveListener(UnityAction<T> listener)
    {
        _onDataUpdated.RemoveListener(listener);
    }
}
