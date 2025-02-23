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

    /// <summary>
    /// 최초 활성화 시 호출되는 메서드
    /// </summary>
    protected void Start()
    {
        if (TryGetComponent(out _model) == false)
        {
            Debug.LogError("Model이 null 입니다");
            return;
        }

        if (TryGetComponent(out _view) == false)
        {
            Debug.LogError("View가 null 입니다");
            return;
        }
        
        Initialize();
    }

    /// <summary>
    /// Presenter의 초기화를 진행하는 메서드
    /// </summary>
    protected virtual void Initialize()
    {
        _model.PropertyChanged += OnModelPropertyChanged;
        
        _view.Initialize(this);
        _model.Initialize();
    }
    
    /// <summary>
    /// View의 ShowView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void ShowView()
    {
        if (_view == null)
        {
            Debug.LogWarning("[PresenterBase] View를 활성화 할 수 없음, View가 Null임");
            return;
        }
        
        _view.ShowView();
    }

    /// <summary>
    /// View를 HideView 메서드를 호출하는 메서드
    /// </summary>
    public virtual void HideView()
    {
        if (_view == null)
        { 
            Debug.LogWarning("[PresenterBase] View를 비활성화 할 수 없음, View가 Null임");
            return;
        }
        
        _view.HideView();
    }

    public TModel GetModel() => _model;

    public void InvokeMethod(Enum methodType)
    {
        if (_model == null)
        {
            Debug.LogError("[PresenterBase] Model 내 메서드 호출 실패, Model이 Null임");
            return;
        }

        _model.InvokeMethod(methodType);
    }
    
    protected void ForceUpdateView(string propertyName) => _view.UpdateView(propertyName);

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        _view.UpdateView(e.PropertyName);
    }
}