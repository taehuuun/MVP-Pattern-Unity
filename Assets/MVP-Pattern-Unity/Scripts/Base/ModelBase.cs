using UnityEngine;
using UnityEngine.Events;

public abstract class ModelBase : MonoBehaviour, IModel
{
    private readonly UnityEvent<ModelBase> _onDataUpdated = new();

    public virtual void Initialize()
    {
        
    }
    
    public void AddListener(UnityAction<ModelBase> listener)
    {
        _onDataUpdated.AddListener(listener);
    }

    public void RemoveListener(UnityAction<ModelBase> listener)
    {
        _onDataUpdated.RemoveListener(listener);
    }

    public void ClearListener()
    {
        _onDataUpdated.RemoveAllListeners();
    }

    public void TriggerDataChange(ModelBase data)
    {
        _onDataUpdated?.Invoke(data);
    }
}
