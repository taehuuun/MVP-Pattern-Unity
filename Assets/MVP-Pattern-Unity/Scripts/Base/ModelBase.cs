using UnityEngine;
using UnityEngine.Events;

public abstract class ModelBase : MonoBehaviour, IModel
{
    private readonly UnityEvent<ModelBase> _onModelChanged = new();

    public virtual void Initialize()
    {
        
    }
    
    public void AddListener(UnityAction<ModelBase> listener)
    {
        _onModelChanged.AddListener(listener);
    }

    public void RemoveListener(UnityAction<ModelBase> listener)
    {
        _onModelChanged.RemoveListener(listener);
    }

    public void ClearListener()
    {
        _onModelChanged.RemoveAllListeners();
    }

    public void TriggerEvent()
    {
        _onModelChanged?.Invoke(this);
    }
}
