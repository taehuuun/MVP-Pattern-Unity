using UnityEngine.Events;

public interface IModel<T>
{
    void AddListener(UnityAction<T> listener);
    void RemoveListener(UnityAction<T> listener);
    void ClearListener();
    void TriggerDataChange(T data);
}