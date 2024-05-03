using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IView 인터페이스를 상속 받는 모든 View의 부모 추상 클래스
/// </summary>
public abstract class ViewBase<TModel> : MonoBehaviour, IView<TModel> where TModel : ModelBase
{
    private PresenterBase<TModel> _presenter;

    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize(PresenterBase<TModel> presenter)
    {
        _presenter = presenter;
    }
    
    /// <summary>
    /// Model 변경 시 View를 업데이트 하는 메서드
    /// </summary>
    /// <param name="changedModel">변경 모델</param>
    public abstract void UpdateView(TModel changedModel);

    /// <summary>
    /// eventName에 해당하는 이벤트를 발생 시키는 메서드
    /// </summary>
    /// <param name="eventName">발생 시킬 이벤트 명</param>
    protected virtual void TriggerEvent(string eventName)
    {
        if (_presenter.Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent unityEvent)
            {
                unityEvent.Invoke();
            }
        }
    }

    /// <summary>
    /// eventName에 해당하는 인자를 1개를 가지는 이벤트를 발생 시키는 메서드
    /// </summary>
    /// <param name="eventName">발생 시킬 이벤트 명</param>
    /// <param name="args">이벤트 발생 시 인자</param>
    /// <typeparam name="T">모든 타입</typeparam>
    public virtual void TriggerEvent<T>(string eventName, T args)
    {
        if (_presenter.Events.TryGetValue(eventName, out var unityEventBase))
        {
            if (unityEventBase is UnityEvent<T> unityEvent)
            {
                unityEvent.Invoke(args);
            }
        }
    }
}
