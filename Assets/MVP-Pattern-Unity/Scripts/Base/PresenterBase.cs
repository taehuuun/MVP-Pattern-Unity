using UnityEngine;

public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
{
    [SerializeField] private ModelBase<T> model;
    [SerializeField] private ViewBase<T> view;

    protected void Start()
    {
        Initialize();
        
        AddViewListeners();
    }

    protected void OnDestroy()
    {
        model.ClearListener();
    }

    public virtual void Initialize()
    {
        model.AddListener(HandleModelUpdate);
    }

}
