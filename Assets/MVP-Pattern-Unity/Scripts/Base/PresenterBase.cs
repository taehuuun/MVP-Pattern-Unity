using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
public abstract class PresenterBase : MonoBehaviour, IPresenter 
{
    private readonly MvpMethodBridge _mvpMethodBridge = new();
    private ModelBase _model;                // ModelBase를 상속 받는 model 필드
    private ViewBase _view;       // ViewBase를 상속 받는 view 필드

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
    public virtual void Initialize()
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
        _model.Initialize(this);
        _view.Initialize(this);
        
        _model.InitializeModelMethods();
        _view.InitializeViewMethod();
        
        InvokeMethod(ViewBaseMethodType.InitializeBindComponent);
        InvokeMethod(ViewBaseMethodType.InitializeEventListeners);
        InvokeMethod(ModelBase.BaseMethodType.InitializeNestedProperties);
        InvokeMethod(ModelBase.BaseMethodType.InitializeProperties);
        InvokeMethod(ViewBaseMethodType.SetupView);
        
        InvokeMethod(ModelBase.BaseMethodType.InitializeInvokeMethod);
        _initialized = true;
    }

    /// <summary>
    /// View의 ShowView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void ShowView()
    {
        if (_view == null)
        {
            Debug.LogError($"[{GetType().Name}] View가 null 입니다");
            return;
        }
        
        InvokeMethod(ViewBaseMethodType.ShowView);
    }

    /// <summary>
    /// View를 HideView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void HideView()
    {
        if (_view == null)
        {
            Debug.LogError($"[{GetType().Name}] View가 null 입니다");
            return;
        }
        
        InvokeMethod(ViewBaseMethodType.HideView);
    }

    public void AddMethod(Enum methodType, Delegate action) => _mvpMethodBridge.AddMethod(methodType, action);
    public void RemoveMethod(Enum methodType) => _mvpMethodBridge.RemoveMethod(methodType);
    public void InvokeMethod(Enum methodType, params object[] parameters) => _mvpMethodBridge.InvokeMethod(methodType, parameters);
    public TResult InvokeMethod<TResult>(Enum methodType, params object[] parameters) => _mvpMethodBridge.InvokeMethod<TResult>(methodType, parameters);
    
    public T GetModelProperty<T>(string propertyName) => _model.GetProperty<T>(propertyName);
    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_view == null)
        {
            Debug.LogError($"[{GetType().Name}] View가 null 입니다");
            return;
        }
        
        InvokeMethod(ViewBaseMethodType.UpdateView, e.PropertyName);;
    }
}