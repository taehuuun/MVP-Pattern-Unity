/// <summary>
/// 모든 뷰 클래스 구현 시 상속 되어야 하는 인터페이스
/// </summary>
public interface IView<TModel> where TModel : ModelBase
{
    /// <summary>
    /// 모델의 변경 시 뷰를 업데이트 하는 메서드
    /// </summary>
    /// <param name="changedModel">변경 된 모델</param>
    void UpdateView(TModel changedModel);
}
