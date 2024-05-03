/// <summary>
/// 모든 뷰 클래스 구현 시 상속 되어야 하는 인터페이스
/// </summary>
public interface IView
{
    void UpdateView(ModelBase changedModel);
}
