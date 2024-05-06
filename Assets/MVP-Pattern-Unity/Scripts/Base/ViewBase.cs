using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// IView 인터페이스를 상속 받는 모든 View의 부모 추상 클래스
/// </summary>
public abstract class ViewBase<TModel> : MonoBehaviour where TModel : ModelBase
{
    /// <summary>
    /// View의 초기화를 진행하는 메서드
    /// </summary>
    public virtual void Initialize(PresenterBase<TModel> presenter)
    {
    }
    
    /// <summary>
    /// Model 변경 시 View를 업데이트 하는 메서드
    /// </summary>
    /// <param name="changedModel">변경 모델</param>
    public abstract void UpdateView(TModel changedModel);
}
