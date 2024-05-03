using UnityEngine;

/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
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
        view.RemoveAllEvent();
    }

    public virtual void Initialize()
    {
        model.Initialize();
        view.Initialize();
        
        model.AddListener(HandleModelUpdate);
        
        model.TriggerEvent();
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

    protected virtual void HandleModelUpdate(ModelBase changedModel)
    {
        view.UpdateView(changedModel);
    }
}