using UnityEngine;

public abstract class ModelBase<T> : MonoBehaviour, IModel<T>
{
    public T Data { get; private set; }
}
