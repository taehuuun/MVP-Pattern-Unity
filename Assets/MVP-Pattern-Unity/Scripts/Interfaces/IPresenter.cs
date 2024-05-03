/// <summary>
/// 모든 Presenter 클래스에 상속되어야 하는 인터페이스
/// </summary>
public interface IPresenter
{
    void Initialize();
    void AddViewListeners();
    void ShowView();
    void HideView();
}