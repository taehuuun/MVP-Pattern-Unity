public interface IPresenter
{
    void Initialize();
    void AddViewListeners();
    void ShowView();
    void HideView();
}