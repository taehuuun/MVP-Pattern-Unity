using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IView 인터페이스를 상속 받는 모든 View의 부모 추상 클래스
/// </summary>
public abstract class ViewBase : MonoBehaviour, IView
{
    // View의 상호작용 이벤트를 관리하는 딕셔너리 필드
    private readonly Dictionary<string, UnityEventBase> _events = new();

    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public abstract void Initialize();
    
    /// <inheritdoc cref="IView.UpdateView"/>
    public abstract void UpdateView(ModelBase changedModel);

    /// <summary>
    /// eventName에 해당하는 이벤트에 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    public virtual void AddListener(string eventName, UnityAction listener)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent());
        }

        ((UnityEvent)_events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트에 인자를 1개 가지는 리스너를 추가 시키는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">추가 할 리스너</param>
    /// <typeparam name="T">리스너 인자</typeparam>
    public virtual void AddListener<T>(string eventName, UnityAction<T> listener)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new UnityEvent<T>());
        }

        ((UnityEvent<T>)_events[eventName]).AddListener(listener);
    }

    /// <summary>
    /// eventName에 해당하는 이벤트의 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="eventName">이벤트 명</param>
    /// <param name="listener">제거 할 리스너</param>
    public virtual void RemoveListener(string eventName, UnityAction listener)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
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
    public virtual void RemoveListener<T>(string eventName, UnityAction<T> listener)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
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
    public virtual void RemoveEvent(string eventName)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            unityEventBase.RemoveAllListeners();
        }
        
        _events.Remove(eventName);
    }

    /// <summary>
    /// _events 딕셔너리의 모든 이벤트 및 리스너를 제거하는 메서드
    /// </summary>
    public virtual void RemoveAllEvent()
    {
        foreach (var events in _events.Values)
        {
            events.RemoveAllListeners();
        }

        _events.Clear();
    }

    /// <summary>
    /// eventName에 해당하는 이벤트를 발생 시키는 메서드
    /// </summary>
    /// <param name="eventName">발생 시킬 이벤트 명</param>
    public virtual void TriggerEvent(string eventName)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.Invoke();
            }
        }
    }

    public virtual void TriggerEvent<T>(string eventName, T args)
    {
        if (_events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<T> unityEvent)
            {
                unityEvent.Invoke(args);
            }
        }
    }
}
