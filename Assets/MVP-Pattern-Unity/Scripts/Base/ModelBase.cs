using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IModel 인터페이스를 상속 받는 모든 모델의 부모 추상 클래스
/// </summary>
public abstract class ModelBase : MonoBehaviour, IModel
{
    private readonly UnityEvent<ModelBase> _onModelChanged = new();     // 모델 변경 시 발생하는 이벤트

    /// <summary>
    /// Model의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize()
    {
        
    }
    
    public void AddListener(UnityAction<ModelBase> listener)
    {
        _onModelChanged.AddListener(listener);
    }

    public void RemoveListener(UnityAction<ModelBase> listener)
    {
        _onModelChanged.RemoveListener(listener);
    }

    public void ClearListener()
    {
        _onModelChanged.RemoveAllListeners();
    }

    public void TriggerEvent()
    {
        _onModelChanged?.Invoke(this);
    }
}
