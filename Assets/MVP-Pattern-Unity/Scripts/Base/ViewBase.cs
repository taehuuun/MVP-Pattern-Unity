using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum ViewBaseMethodType
{
    InitializeBindComponent,
    InitializeEventListeners,
    SetupView,
    UpdateView,
    HideView,
    ShowView,
}

public abstract class ViewBase : MonoBehaviour
{
    private PresenterBase _presenter;
    
    private readonly Dictionary<Type, List<Object>> _bindObjects = new();

    public void InitializeViewMethod()
    {
        AddMethod(ViewBaseMethodType.InitializeBindComponent, (Action)InitializeBindComponent);
        AddMethod(ViewBaseMethodType.InitializeEventListeners, (Action)InitializeEventListeners);
        AddMethod(ViewBaseMethodType.SetupView, (Action)SetupView);
        AddMethod(ViewBaseMethodType.UpdateView, (Action<string>)UpdateView);
        AddMethod(ViewBaseMethodType.HideView, (Action)HideView);
        AddMethod(ViewBaseMethodType.ShowView, (Action)ShowView);
    }

    public void InitializePresenter(PresenterBase presenter) => _presenter = presenter;
    protected virtual void InitializeBindComponent() { }
    protected virtual void InitializeEventListeners() { }
    protected virtual void SetupView() { }

    /// <summary>
    /// 프로퍼티 변경 시 해당하는 UI를 업데이트 하는 메서드
    /// </summary>
    /// <param name="propertyName">변경 프로퍼티 명</param>
    public virtual void UpdateView(string propertyName) { }

    /// <summary>
    /// View를 활성화 시키는 메서드
    /// </summary>
    protected virtual void ShowView() => gameObject.SetActive(true);

    /// <summary>
    /// View를 비활성화 시키는 메서드
    /// </summary>
    protected virtual void HideView() => gameObject.SetActive(false);
    
    
    #region Util

    private void AddMethod(Enum methodType, Delegate method) => _presenter.AddMethod(methodType, method);
    protected void InvokeMethod(Enum methodType, params object[] parameters) => _presenter.InvokeMethod(methodType, parameters);
    protected TResult InvokeMethod<TResult>(Enum methodType, params object[] parameters) => _presenter.InvokeMethod<TResult>(methodType, parameters);
    
    protected T GetProperty<T>(string propertyName) => _presenter.GetModelProperty<T>(propertyName);
    
    protected virtual void Bind<T>(Type enumType) where T : Object
    {
        if (!_bindObjects.ContainsKey(typeof(T)))
        {
            _bindObjects[typeof(T)] = new List<Object>();
        }

        foreach (var enumName in Enum.GetNames(enumType))
        {
            Object component = typeof(T) == typeof(GameObject)
                ? FindComponent(gameObject, enumName, true)
                : FindComponent<T>(gameObject, enumName, true);

            if (component != null)
            {
                _bindObjects[typeof(T)].Add(component);
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}] {gameObject.name}에서 {enumName} ({typeof(T)})을(를) 찾을 수 없습니다.");
            }
        }

    }
    protected T GetBind<T>(int index) where T : Object
    {
        if (_bindObjects.TryGetValue(typeof(T), out var objects) && objects.Count > index && index >= 0)
        {
            return objects[index] as T;
        }
        
        Debug.LogWarning($"[{GetType().Name}] {gameObject.name}: {typeof(T)}에 대한 잘못된 바인딩 인덱스 ({index})");
        return null;
    }

    private GameObject FindComponent(GameObject root, string name, bool recursive = false) => FindComponent<Transform>(root, name, recursive)?.gameObject;
    private T FindComponent<T>(GameObject root, string name, bool recursive = false) where T : Object
    {
        if (root == null)
        {
            return null;
        }

        if (recursive)
        {
            foreach (T component in root.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }
        else
        {
            foreach (Transform child in root.transform)
            {
                if (child.name == name && child.TryGetComponent(out T component))
                {
                    return component;
                }
            }
        }

        Debug.LogWarning($"[{GetType().Name}] {typeof(T)} ({name})이(가) {root.name}에 없습니다.");
        return null;
    }

    #endregion
}
