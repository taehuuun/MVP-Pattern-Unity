using UnityEngine;

public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
{
    [SerializeField] private ModelBase<T> model;
    [SerializeField] private ViewBase<T> view;

    public virtual void HideView()
    {
        view.gameObject.SetActive(false);
    }

    protected virtual void HandleModelUpdate(T data)
    {
        view.UpdateView(data);
    }
}