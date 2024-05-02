using UnityEngine;

public abstract class PresenterBase : MonoBehaviour, IPresenter 
{
    [SerializeField] protected ModelBase model;
    [SerializeField] protected ViewBase view;

    protected void Start()
    {
        Initialize();
        
        AddViewListeners();
    }

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

    protected virtual void HandleModelUpdate(ModelBase model)
    {
        view.UpdateView(model);
    }
}