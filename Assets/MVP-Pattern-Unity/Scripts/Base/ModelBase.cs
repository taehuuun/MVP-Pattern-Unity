using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// IModel 인터페이스를 상속 받는 모든 모델의 부모 추상 클래스
/// </summary>
public abstract class ModelBase : MonoBehaviour, INotifyPropertyChanged
{
    public enum BaseMethodType
    {
        InitializeProperties,
        InitializeNestedProperties,
        InitializeInvokeMethod
    }
    public event PropertyChangedEventHandler PropertyChanged;
    
    private PresenterBase _presenter;
    
    private readonly Dictionary<string, object> _propertyStorage = new();

    #region Initialize

    public void Initialize(PresenterBase presenter) => _presenter = presenter;
    
    /// <summary>
    /// 초기화 시 모델 내 메서드를 추가하는 메서드
    /// </summary>
    public virtual void InitializeModelMethods()
    {
        AddMethod(BaseMethodType.InitializeProperties, (Action)InitializeProperties);
        AddMethod(BaseMethodType.InitializeNestedProperties, (Action)InitializeNestedProperties);
        AddMethod(BaseMethodType.InitializeInvokeMethod, (Action)InitializeInvokeMethod);
    }

    protected virtual void InitializeNestedProperties() { }
    
    /// <summary>
    /// 초기화 시 모델 내 프로퍼티를 초기화하는 메서드
    /// </summary>
    protected virtual void InitializeProperties() { }

    protected virtual void InitializeInvokeMethod() { }

    #endregion

    protected void AddMethod(Enum methodType, Delegate unityAction) => _presenter.AddMethod(methodType, unityAction);
    protected void RemoveMethod(Enum methodType) => _presenter.RemoveMethod(methodType);
    
    #region PropertyChange

    /// <summary>
    /// 지정된 이름의 프로퍼티 값을 가져오는 메서드
    /// </summary>
    /// <param name="propertyName">가져올 프로퍼티의 이름</param>
    /// <typeparam name="T">프로퍼티 값의 타입</typeparam>
    /// <returns>해당 프로퍼티의 값, 값이 없을 경우 기본값 반환</returns>
    public T GetProperty<T>([CallerMemberName]string propertyName = null)
    {
        if (propertyName != null && _propertyStorage.TryGetValue(propertyName, out var value))
        {
            return (T) value;
        }
        
        return default;
    }

    /// <summary>
    /// 등록된 프로퍼티를 변경 및 PropertyChanged 이벤트를 발생시키는 메서드
    /// </summary>
    /// <typeparam name="T">업데이트할 속성의 데이터 타입</typeparam>
    /// <param name="value">변경 프로퍼티 값</param>
    /// <param name="propertyName">변경 프로퍼티 이름</param>
    protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
    {
        if (propertyName == null)
        {
            return;
        }
        
        if (_propertyStorage.TryGetValue(propertyName, out var currentValue) && EqualityComparer<T>.Default.Equals((T)currentValue, value))
        {
            return;
        }

        if (currentValue is BindableData oldBindValue)
        {
            oldBindValue.PropertyChanged -= OnNestedPropertyChanged;
        }

        if (value is BindableData newBindValue)
        {
            newBindValue.PropertyChanged += OnNestedPropertyChanged;
        }

        _propertyStorage[propertyName] = value;
        OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// 중첩된 속성 값 변경 시 호출되는 메서드
    /// </summary>
    /// <param name="sender">변경 이벤트를 발생시킨 객체</param>
    /// <param name="e">속성 변경 관련 이벤트 데이터</param>
    protected virtual void OnNestedPropertyChanged(object sender, PropertyChangedEventArgs e) { }

    /// <summary>
    /// 속성 값 변경 시 변경 내용을 알리기 위해 PropertyChanged 이벤트를 호출하는 메서드
    /// </summary>
    /// <param name="propertyName">변경된 속성의 이름</param>
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    #endregion
}