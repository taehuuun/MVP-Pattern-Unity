using UnityEngine.Events;

public interface IModelBase<T>
{
    void AddListener(UnityAction<T> listener);
    void RemoveListener(UnityAction<T> listener);
}