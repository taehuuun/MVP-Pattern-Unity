using UnityEngine;

public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
{
    [SerializeField] private ModelBase<T> model;
    [SerializeField] private ViewBase<T> view;

    protected virtual void OnDestroy()
    {
        model.ClearListener();
    }

    public virtual void Initialize()
    {
        model.AddListener(HandleModelUpdate);
    }

    public virtual void AddViewListeners()
    {
        
    }

    public virtual void ShowView()
    {
        view.gameObject.SetActive(true);
    }

    public virtual void HideView()
    {
        view.gameObject.SetActive(false);
    }

    protected virtual void HandleModelUpdate(T data)
    {
        view.UpdateView(data);
    }
}