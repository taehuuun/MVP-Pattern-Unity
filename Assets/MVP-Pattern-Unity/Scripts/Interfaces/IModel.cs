using UnityEngine.Events;

/// <summary>
/// 모든 모델 클래스에 상속되어야 하는 인터페이스
/// </summary>
public interface IModel
{
    void AddListener(UnityAction<ModelBase> listener);
    void RemoveListener(UnityAction<ModelBase> listener);
    void ClearListener();
    
    /// <summary>
    /// 모델 내 등록 된 이벤트를 발생 시키는 메서드
    /// </summary>
    void TriggerEvent();
}