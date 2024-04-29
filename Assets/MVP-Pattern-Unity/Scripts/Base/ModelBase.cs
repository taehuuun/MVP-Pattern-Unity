using UnityEngine;

public abstract class ModelBase<T> : MonoBehaviour, IModel<T>
{
    public T Data { get; private set; }
    
    private readonly UnityEvent<T> _onDataUpdated = new();
    
    public virtual void AddListener(UnityAction<T> listener)
    {
        _onDataUpdated.AddListener(listener);
    }

    public virtual void RemoveListener(UnityAction<T> listener)
    {
        _onDataUpdated.RemoveListener(listener);
    }

    public virtual void ClearListener()
    {
        _onDataUpdated.RemoveAllListeners();
    }

    public virtual void TriggerDataChange(T data)
    {
        _onDataUpdated?.Invoke(data);
    }
}
