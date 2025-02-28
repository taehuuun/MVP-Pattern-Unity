using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
public abstract class PresenterBase<TModel> : MonoBehaviour, IPresenter where TModel : ModelBase 
{
    private TModel _model;                // ModelBase를 상속 받는 model 필드
    private ViewBase<TModel> _view;       // ViewBase를 상속 받는 view 필드

    private bool _initialized;

    /// <summary>
    /// 최초 활성화 시 호출되는 메서드
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Presenter의 초기화를 진행하는 메서드
    /// </summary>
    protected virtual void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        
        if (TryGetComponent(out _model) == false)
        {
            Debug.LogError($"[{GetType().Name}] Model이 null 입니다");
            return;
        }

        if (TryGetComponent(out _view) == false)
        {
            Debug.LogError($"[{GetType().Name}] View가 null 입니다");
            return;
        }
        
        _model.PropertyChanged += OnModelPropertyChanged;
        
        _view.Initialize(this);
        _model.Initialize();
        
        InvokeMethod(ModelBase.BaseMethodType.InitializeProperties);
        
        _initialized = true;
    }
    
    /// <summary>
    /// View의 ShowView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void ShowView() => ExecuteSafe(_view, v => v.ShowView());

    /// <summary>
    /// View를 HideView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void HideView() => ExecuteSafe(_view, v => v.HideView());

    public TModel GetModel() => _model;
    public void InvokeMethod(Enum methodType) 
        => ExecuteSafe(_model, m => m.InvokeMethod(methodType));
    public void InvokeMethod<TParam>(Enum methodType, TParam param) 
        => ExecuteSafe(_model, m => m.InvokeMethod(methodType, param));
    public void InvokeMethod<TParam1, TParam2>(Enum methodType, TParam1 param, TParam2 param2) 
        => ExecuteSafe(_model, m => m.InvokeMethod(methodType, param, param2));
    public TResult InvokeMethod<TResult>(Enum methodType) 
        => ExecuteSafe(_model, m => m.InvokeMethod<TResult>(methodType));
    public TResult InvokeMethod<TParam, TResult>(Enum methodType, TParam param) 
        => ExecuteSafe(_model, m => m.InvokeMethod<TParam, TResult>(methodType, param));
    
    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e) => ExecuteSafe(_view, v => v.UpdateView(e.PropertyName));
    private void ExecuteSafe<T>(T obj, Action<T> action) where T : class
    {
        if (obj == null)
        {
            Debug.LogWarning($"[{GetType().Name}] {typeof(T).Name}가 Null임");
            return;
        }
        action(obj);
    }
    private TResult ExecuteSafe<T, TResult>(T obj, Func<T, TResult> func) where T : class
    {
        if (obj == null)
        {
            Debug.LogError($"[{GetType().Name}] {typeof(T).Name}가 Null임");
            return default;
        }
        return func(obj);
    }
}