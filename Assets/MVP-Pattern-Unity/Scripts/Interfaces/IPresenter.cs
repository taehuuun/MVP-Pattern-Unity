/// <summary>
/// 모든 Presenter 클래스에 상속되어야 하는 인터페이스
/// </summary>
public interface IPresenter
{
    /// <summary>
    /// Presenter의 초기화를 진행하는 메서드
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// View에 리스너틑 추가하는 메서드
    /// </summary>
    void AddViewListeners();
    
    /// <summary>
    /// View를 활성화 시키는 메서드
    /// </summary>
    void ShowView();
    
    /// <summary>
    /// View를 비활성화 시키는 메서드
    /// </summary>
    void HideView();
}