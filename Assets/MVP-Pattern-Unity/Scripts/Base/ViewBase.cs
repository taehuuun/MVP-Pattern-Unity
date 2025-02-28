using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
{
    private readonly Dictionary<Type, List<Object>> _bindObjects = new();
    
    protected PresenterBase<TModel> p_presenter;
    
    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public void Initialize(PresenterBase<TModel> presenter)
    {
        p_presenter = presenter;
        InitializeBind();
        InitializeEvents(presenter);
    }
    
    protected virtual void InitializeBind() { }
    
    protected virtual void InitializeEvents(PresenterBase<TModel> presenter) { }
    
    /// <summary>
    /// Model 변경 시 View를 업데이트 하는 메서드
    /// </summary>
    /// <param name="propertyName">변경 모델</param>
    public abstract void UpdateView(string propertyName);
    
    /// <summary>
    /// View를 활성화 시키는 메서드
    /// </summary>
    public virtual void ShowView() => gameObject.SetActive(true);

    /// <summary>
    /// View를 비활성화 시키는 메서드
    /// </summary>
    public virtual void HideView() => gameObject.SetActive(false);

    /// <summary>
    /// 현재 Presenter의 Model 가져오기
    /// </summary>
    protected TModel GetModel() => p_presenter.GetModel();
    
    protected virtual void Bind<T>(Type enumType) where T : Object
    {
        if (!_bindObjects.ContainsKey(typeof(T)))
        {
            _bindObjects[typeof(T)] = new List<Object>();
        }

        foreach (string name in Enum.GetNames(enumType))
        {
            Object component = typeof(T) == typeof(GameObject)
                ? FindComponent(gameObject, name, true)
                : FindComponent<T>(gameObject, name, true);

            if (component != null)
            {
                _bindObjects[typeof(T)].Add(component);
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}] {gameObject.name}에서 {name} ({typeof(T)})을(를) 찾을 수 없습니다.");
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
}
