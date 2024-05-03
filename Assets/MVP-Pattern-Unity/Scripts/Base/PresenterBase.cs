using UnityEngine;

/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
public abstract class PresenterBase : MonoBehaviour, IPresenter 
{
    [SerializeField] protected ModelBase model;     // ModelBase를 상속 받는 model 필드
    [SerializeField] protected ViewBase view;       // ViewBase를 상속 받는 view 필드

    /// <summary>
    /// 최초 활성화 시 호출되는 메서드
    /// </summary>
    protected void Start()
    {
        Initialize();
        
        AddViewListeners();
    }

    /// <summary>
    /// 오브젝트 파괴 시 호출되는 메서드
    /// </summary>
    protected virtual void OnDestroy()
    {
        model.ClearListener();
        view.RemoveAllEvent();
    }

    /// <inheritdoc cref="IPresenter.Initialize"/>
    public virtual void Initialize()
    {
        model.Initialize();
        view.Initialize();
        
        model.AddListener(HandleModelUpdate);
        
        model.TriggerEvent();
    }

    /// <inheritdoc cref="IPresenter.AddViewListeners"/>
    public virtual void AddViewListeners()
    {
        
    }

    /// <inheritdoc cref="IPresenter.ShowView"/>
    public virtual void ShowView()
    {
        view.gameObject.SetActive(true);
    }

    /// <inheritdoc cref="IPresenter.HideView"/>
    public virtual void HideView()
    {
        view.gameObject.SetActive(false);
    }

    /// <summary>
    /// 모델이 변경될 때 호출 되는 메서드
    /// </summary>
    /// <param name="changedModel">변경 된 모델</param>
    protected virtual void HandleModelUpdate(ModelBase changedModel)
    {
        view.UpdateView(changedModel);
    }
}