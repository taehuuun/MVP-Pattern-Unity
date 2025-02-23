using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// IModel 인터페이스를 상속 받는 모든 모델의 부모 추상 클래스
/// </summary>
public abstract class ModelBase : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;               // 모델 내 프로퍼티 변경 감지 시 발생시키는 이벤트
    
    private readonly Dictionary<string, object> _propertyStorage = new();
    private readonly Dictionary<Enum, Delegate> _modelMethods = new();

    /// <summary>
    /// Model의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize() { }
    
    /// <summary>
    /// 반환 값 X, 매개변수가 없는 메서드를 실행하는 메서드
    /// </summary>
    /// <param name="methodType">실행할 메서드의 타입</param>
    public void InvokeMethod(Enum methodType)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Action action)
            {
                action();
            }
            else
            {
                Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Action).");
            }
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }
    
    /// <summary>
    /// 반환 값 X, 매개변수가 1개인 메서드를 실행하는 메서드
    /// </summary>
    /// <param name="methodType">호출할 메서드의 타입</param>
    /// <param name="param">매개변수</param>
    /// <typeparam name="TParam">매개변수 타입</typeparam>
    public void InvokeMethod<TParam>(Enum methodType, TParam param)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Action<TParam> action)
            {
                action(param);
            }
            else
            {
                Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Action).");
            }
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }
    
    /// <summary>
    /// 반환 값 X, 매개변수가 2개인 메서드를 실행하는 메서드
    /// </summary>
    /// <param name="methodType">실행 할 메서드의 타입</param>
    /// <param name="param1">첫 번째 매개변수</param>
    /// <param name="param2">두 번째 매개변수</param>
    /// <typeparam name="TParam1">첫 번째 매개변수의 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수의 타입</typeparam>
    public void InvokeMethod<TParam1, TParam2>(Enum methodType, TParam1 param1, TParam2 param2)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Action<TParam1, TParam2> action)
            {
                action(param1, param2);
            }
            else
            {
                Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Action).");
            }
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }
    
    /// <summary>
    /// 반환 값 X, 매개변수가 3개인 메서드를 실행하는 메서드 
    /// </summary>
    /// <param name="methodType">실행 할 메서드의 타입</param>
    /// <param name="param1">첫 번째 매개변수</param>
    /// <param name="param2">두 번째 매개변수</param>
    /// <param name="param3">세 번째 매개변수</param>
    /// <typeparam name="TParam1">첫 번째 매개변수의 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수의 타입</typeparam>
    /// <typeparam name="TParam3">세 번째 매개변수의 타입</typeparam>
    public void InvokeMethod<TParam1, TParam2, TParam3>(Enum methodType, TParam1 param1, TParam2 param2, TParam3 param3)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Action<TParam1, TParam2, TParam3> action)
            {
                action(param1, param2, param3);
            }
            else
            {
                Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Action).");
            }
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }
    
    /// <summary>
    /// 반환 값 O, 매개변수가 없는 메서드를 실행하는 메서드 
    /// </summary>
    /// <param name="methodType">실행할 메서드 타입</param>
    /// <typeparam name="TResult">반환 타입</typeparam>
    /// <returns>메서드 실행 후 결과</returns>
    public TResult InvokeMethod<TResult>(Enum methodType)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Func<TResult> func)
            {
                return func();
            }
    
            Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Func).");
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
        
        return default;
    }
    
    /// <summary>
    /// 반환 값 O, 매개변수가 1개인 메서드를 실행하는 메서드 
    /// </summary>
    /// <param name="methodType">실행할 메서드 타입</param>
    /// <param name="param">매개변수</param>
    /// <typeparam name="TParam">매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    /// <returns>메서드 실행 후 결과</returns>
    public TResult InvokeMethod<TParam, TResult>(Enum methodType, TParam param)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Func<TParam, TResult> func)
            {
                return func(param);
            }
    
            Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Func).");
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
        
        return default;
    }
    
    /// <summary>
    /// 반환 값 O, 매개변수가 2개인 메서드를 실행하는 메서드 
    /// </summary>
    /// <param name="methodType">실행할 메서드 타입</param>
    /// <param name="param1">첫 번째 매개변수</param>
    /// <param name="param2">두 번째 매개변수</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    /// <returns>메서드 실행 후 결과</returns>
    public TResult InvokeMethod<TParam1, TParam2, TResult>(Enum methodType, TParam1 param1, TParam2 param2)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Func<TParam1, TParam2, TResult> func)
            {
                return func(param1, param2);
            }
            
            Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Func).");
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
        
        return default;
    }
    
    /// <summary>
    /// 반환 값 O, 매개변수가 3개인 메서드를 실행하는 메서드 
    /// </summary>
    /// <param name="methodType">실행할 메서드 타입</param>
    /// <param name="param1">첫 번째 매개변수</param>
    /// <param name="param2">두 번째 매개변수</param>
    /// <param name="param3">세 번째 매개변수</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam3">세 번째 매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    /// <returns>메서드 실행 후 결과</returns>
    public TResult InvokeMethod<TParam1, TParam2, TParam3, TResult>(Enum methodType, TParam1 param1, TParam2 param2, TParam3 param3)
    {
        if (_modelMethods.TryGetValue(methodType, out var method))
        {
            if (method is Func<TParam1, TParam2, TParam3, TResult> func)
            {
                return func(param1, param2, param3);
            }
            
            Debug.LogError($"메서드 타입 {methodType}에 호환되지 않는 시그니처(expected Func).");
        }
        else
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
        
        return default;
    }

    /// <summary>
    /// 반환 값 X, 매개변수가 없는 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="action">추가할 메서드</param>
    protected void AddMethod(Enum methodType, Action action) => RegisterMethod(methodType, action);

    /// <summary>
    /// 반환 O, 매개변수가 없는 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="func">추가할 메서드</param>
    /// <typeparam name="TResult">반환 타입</typeparam>
    protected void AddMethod<TResult>(Enum methodType, Func<TResult> func) => RegisterMethod(methodType, func);

    /// <summary>
    /// 반환 값 X, 매개변수가 1개인 메서드를 추가하는 메서드 
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="action">추가할 메서드</param>
    /// <typeparam name="TParam">매개 변수 타입</typeparam>
    protected void AddMethod<TParam>(Enum methodType, Action<TParam> action) => RegisterMethod(methodType, action);
    
    /// <summary>
    /// 반환 값 O, 매개변수가 1개인 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="func">추가할 메서드</param>
    /// <typeparam name="TParam">매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    protected void AddMethod<TParam, TResult>(Enum methodType, Func<TParam, TResult> func) => RegisterMethod(methodType, func);
    
    /// <summary>
    /// 반환 값 X, 매개변수가 2개인 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="action">추가할 메서드</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    protected void AddMethod<TParam1, TParam2>(Enum methodType, Action<TParam1, TParam2> action) => RegisterMethod(methodType, action);
    
    /// <summary>
    /// 반환 값 O, 매개변수가 2개인 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="func">추가할 메서드</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    protected void AddMethod<TParam1, TParam2, TResult>(Enum methodType, Func<TParam1, TParam2, TResult> func) => RegisterMethod(methodType, func);
    
    /// <summary>
    /// 반환 값 X, 매개변수가 3개인 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="action">추가할 메서드</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam3">세 번째 매개변수 타입</typeparam>
    protected void AddMethod<TParam1, TParam2, TParam3>(Enum methodType, Action<TParam1, TParam2, TParam3> action) => RegisterMethod(methodType, action);
    
    /// <summary>
    /// 반환 값 O, 매개변수가 3개인 메서드를 추가하는 메서드
    /// </summary>
    /// <param name="methodType">추가할 메서드 타입</param>
    /// <param name="func">추가할 메서드</param>
    /// <typeparam name="TParam1">첫 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam2">두 번째 매개변수 타입</typeparam>
    /// <typeparam name="TParam3">세 번째 매개변수 타입</typeparam>
    /// <typeparam name="TResult">반환 타입</typeparam>
    protected void AddMethod<TParam1, TParam2, TParam3, TResult>(Enum methodType, Func<TParam1, TParam2, TParam3, TResult> func) => RegisterMethod(methodType, func);
    
    /// <summary>
    /// 등록되어 있는 메서드 중 타입에 해당하는 메서드를 제거하는 메서드
    /// </summary>
    /// <param name="methodType">제거할 메서드 타입</param>
    protected void RemoveMethod(Enum methodType)
    {
        if (!_modelMethods.Remove(methodType))
        {
            Debug.LogError($"메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }

    /// <summary>
    /// 새로운 메서드를 등록하는 메서드
    /// </summary>
    /// <param name="methodType">등록할 메서드 타입</param>
    /// <param name="unityAction">등록할 메서드</param>
    private void RegisterMethod(Enum methodType, Delegate unityAction)
    {
        if (!_modelMethods.TryAdd(methodType, unityAction))
        {
            Debug.LogError($"메서드 타입{methodType}은(는) 이미 등록 됨");
        }
    }

    /// <summary>
    /// 지정된 이름의 프로퍼티 값을 가져오는 메서드
    /// </summary>
    /// <param name="propertyName">가져올 프로퍼티의 이름</param>
    /// <typeparam name="T">프로퍼티 값의 타입</typeparam>
    /// <returns>해당 프로퍼티의 값, 값이 없을 경우 기본값 반환</returns>
    protected T GetProperty<T>(string propertyName)
    {
        if (_propertyStorage.TryGetValue(propertyName, out var value))
        {
            return (T) value;
        }
        
        return default;
    }

    /// <summary>
    /// 지정된 속성을 업데이트하고, 변경 사항이 있을 경우 PropertyChanged 이벤트를 발생시키는 메서드
    /// </summary>
    /// <typeparam name="T">업데이트할 속성의 데이터 타입</typeparam>
    /// <param name="propertyName">업데이트할 속성의 이름</param>
    /// <param name="value">속성에 설정할 값</param>
    /// <returns>속성이 변경되었을 경우 true, 아니면 false</returns>
    protected bool SetProperty<T>(string propertyName, T value)
    {
        if (_propertyStorage.TryGetValue(propertyName, out var currentValue) && EqualityComparer<T>.Default.Equals((T)currentValue, value))
        {
            return false;
        }

        if (currentValue is BindableModel oldBindValue)
        {
            oldBindValue.PropertyChanged -= OnNestedPropertyChanged;
        }

        if (value is BindableModel newBindValue)
        {
            newBindValue.PropertyChanged += OnNestedPropertyChanged;
        }

        _propertyStorage[propertyName] = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// 중첩된 속성 값 변경 시 호출되는 메서드
    /// </summary>
    /// <param name="sender">변경 이벤트를 발생시킨 객체</param>
    /// <param name="e">속성 변경 관련 이벤트 데이터</param>
    protected void OnNestedPropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);

    /// <summary>
    /// 속성 값 변경 시 변경 내용을 알리기 위해 PropertyChanged 이벤트를 호출하는 메서드
    /// </summary>
    /// <param name="propertyName">변경된 속성의 이름</param>
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}