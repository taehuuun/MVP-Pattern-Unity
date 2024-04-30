using UnityEngine;

public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
{
    [SerializeField] private ModelBase<T> model;

}
