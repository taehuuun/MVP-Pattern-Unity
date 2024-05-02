using UnityEngine;
using UnityEngine.Events;

public abstract class ModelBase<T> : MonoBehaviour, IModel<T>
{
    public T Data { get; protected set; }

    private readonly UnityEvent<T> _onDataUpdated = new();
    
    public void AddListener(UnityAction<T> listener)
    {
        _onDataUpdated.AddListener(listener);
    }

    public void RemoveListener(UnityAction<T> listener)
    {
        _onDataUpdated.RemoveListener(listener);
    }

    public void ClearListener()
    {
        _onDataUpdated.RemoveAllListeners();
    }

    public void TriggerDataChange(T data)
    {
        _onDataUpdated?.Invoke(data);
    }
}
