using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IPresenter 인터페이스를 상속 받는 모든 Presenter의 부모 추상 클래스
/// </summary>
public abstract class PresenterBase : MonoBehaviour, IPresenter 
{
    // View의 상호작용 이벤트를 관리하는 딕셔너리 필드
    public Dictionary<string, UnityEventBase> Events { get; }= new();
    
    [SerializeField] protected ModelBase model;     // ModelBase를 상속 받는 model 필드
    [SerializeField] protected ViewBase view;       // ViewBase를 상속 받는 view 필드

    /// <summary>
    /// 최초 활성화 시 호출되는 메서드
    /// </summary>
    protected void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 오브젝트 파괴 시 호출되는 메서드
    /// </summary>
    protected virtual void OnDestroy()
    {
        model.ClearListener();
        RemoveAllEvent();
    }

    /// <inheritdoc cref="IPresenter.Initialize"/>
    protected virtual void Initialize()
    {
        model.Initialize();
        view.Initialize(this);
        
        model.AddListener(HandleModelUpdate);
        
        model.TriggerEvent();
    }

/// <summary>
    /// eventName에 해당하는 이벤트에 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    protected virtual void AddListener(string eventName, UnityAction listener)
    {
        if (!Events.ContainsKey(eventName))
        {
            Events.Add(eventName, new UnityEvent());
        }

        ((UnityEvent)Events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트에 인자를 1개 가지는 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    /// <typeparam name="T">리스너 인자</typeparam>
    protected virtual void AddListener<T>(string eventName, UnityAction<T> listener)
    {
        if (!Events.ContainsKey(eventName))
        {
            Events.Add(eventName, new UnityEvent<T>());
        }

        ((UnityEvent<T>)Events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트의 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">제거 할 리스너</param>
    protected virtual void RemoveListener(string eventName, UnityAction listener)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }

    /// <summary>
    /// eventName에 해당하는 이벤트의 인자 1개를 가지는 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">제거 할 리스너</param>
    /// <typeparam name="T">제거 할 리스너의 인자</typeparam>
    protected virtual void RemoveListener<T>(string eventName, UnityAction<T> listener)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<T> unityEvent)
            {
                unityEvent.RemoveListener(listener);
            }
        }
    }

    /// <summary>
    /// eventName에 해당하는 이벤트를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">제거 할 이벤트 명</param>
    protected virtual void RemoveEvent(string eventName)
    {
        if (Events.TryGetValue(eventName, out var unityEventBase))
        {
            unityEventBase.RemoveAllListeners();
        }
        
        Events.Remove(eventName);
    }

    /// <summary>
    /// _events 딕셔너리의 모든 이벤트 및 리스너를 제거하는 메서드
    /// </summary>
    protected virtual void RemoveAllEvent()
    {
        foreach (var events in Events.Values)
        {
            events.RemoveAllListeners();
        }

        Events.Clear();
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