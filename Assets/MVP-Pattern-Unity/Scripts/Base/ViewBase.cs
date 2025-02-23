using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// IView 인터페이스를 상속 받는 모든 View의 부모 추상 클래스
/// </summary>
public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
{
    private readonly Dictionary<Type, List<object>> _bindObjects = new();

    private PresenterBase<TModel> _presenter;
    
    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize(PresenterBase<TModel> presenter) => _presenter = presenter;
    
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

    protected TModel GetModel() => _presenter.GetModel();
    
    protected virtual void Bind<T>(Type bindType) where T : Object
    {
        if (!_bindObjects.ContainsKey(typeof(T)))
        {
            _bindObjects.Add(typeof(T), new List<object>());
        }

        foreach (var bindName in Enum.GetNames(bindType))
        {
            var component = FindUI<T>(gameObject, bindName, true);

            if (component == null)
            {
                Debug.LogWarning($"[ViewBase] {gameObject.name}에서 {typeof(T)}을(를) 바인딩하지 못했습니다");
                continue;
            }
            
            _bindObjects[typeof(T)].Add(component);
        }
    }
    protected T GetBindUI<T>(int uiIndex) where T : Object
    {
        if (!_bindObjects.TryGetValue(typeof(T), out var bindUI) || uiIndex < 0 || uiIndex >= bindUI.Count)
        {
            Debug.LogWarning($"{gameObject.name}의 {typeof(T)} 유형에 대한 UI 인덱스 {uiIndex}가 잘못되었습니다");
            return null;
        }
        
        return bindUI[uiIndex] as T;
    }

    private T FindUI<T>(GameObject gameObject, string name, bool recursive = false) where T : Object
    {
        T result = null;
        
        if (gameObject == null)
        {
            return null;
        }

        if (recursive)
        {
            foreach (var component in gameObject.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    result = component;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;

                if(child.name == name && child.TryGetComponent(out T component))
                {
                    result = component;
                    break;
                }
            }
        }

        if (result == null)
        {
            Debug.LogWarning($"[ViewBase] {typeof(T)}타입 ({name})가 {gameObject.name}에 없습니다");
        }
        
        return result;
    }
}
