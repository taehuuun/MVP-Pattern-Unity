using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IModel 인터페이스를 상속 받는 모든 모델의 부모 추상 클래스
/// </summary>
public abstract class ModelBase : MonoBehaviour
{
    private readonly UnityEvent _onModelChanged = new();     // 모델 변경 시 발생하는 이벤트

    /// <summary>
    /// Model의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize()
    {
        
    }
    
    /// <summary>
    /// 모델 내 이벤트에 리스너를 추가하는 메서드
    /// </summary>
    /// <param name="listener">추가 할 리스너</param>
    public void AddListener(UnityAction listener)
    {
        _onModelChanged.AddListener(listener);
    }

    /// <summary>
    /// 모델 내 등록 된 이벤트 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="listener">제거 할 리스너</param>
    public void RemoveListener(UnityAction listener)
    {
        _onModelChanged.RemoveListener(listener);
    }

    /// <summary>
    /// 모델 내 등록 된 모든 리스너를 제거하는 메서드
    /// </summary>
    public void ClearListener()
    {
        _onModelChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 모델 내 등록 된 이벤트를 발생 시키는 메서드
    /// </summary>
    public void TriggerEvent()
    {
        _onModelChanged?.Invoke();
    }
}
