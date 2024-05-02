using UnityEngine.Events;

/// <summary>
/// 모든 모델 클래스에 상속되어야 하는 인터페이스
/// </summary>
public interface IModel
{
    /// <summary>
    /// 모델 내 이벤트에 리스너를 추가하는 메서드
    /// </summary>
    /// <param name="listener">ModelBase 상속 클래스를 매개 받는 UnityAction 리스너</param>
    void AddListener(UnityAction<ModelBase> listener);
    
    /// <summary>
    /// 모델 내 등록 된 이벤트 리스너를 제거하는 메서드
    /// </summary>
    /// <param name="listener">ModelBase 상속 클래스를 매개 받는 UnityAction 리스너</param>
    void RemoveListener(UnityAction<ModelBase> listener);
    void ClearListener();
    
    /// <summary>
    /// 모델 내 등록 된 이벤트를 발생 시키는 메서드
    /// </summary>
    void TriggerEvent();
}