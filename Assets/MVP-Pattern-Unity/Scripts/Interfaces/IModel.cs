using UnityEngine.Events;

public interface IModel
{
    void AddListener(UnityAction<ModelBase> listener);
    void RemoveListener(UnityAction<ModelBase> listener);
    void ClearListener();
    void TriggerEvent();
}