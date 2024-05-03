/// <summary>
/// 모든 Presenter 클래스에 상속되어야 하는 인터페이스
/// </summary>
public interface IPresenter
{
    /// <summary>
    /// Presenter의 초기화를 진행하는 메서드
    /// </summary>
    void Initialize();
    void AddViewListeners();
    void ShowView();
    void HideView();
}