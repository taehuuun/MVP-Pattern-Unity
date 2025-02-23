public class ClickerPresenter : PresenterBase<ClickerModel>
{
    protected override void Initialize()
    {
        base.Initialize();
        
        ForceUpdateView(nameof(ClickerModel.Data.Gold));
        ForceUpdateView(nameof(ClickerModel.Data.GoldPerClickLevel));
        ForceUpdateView(nameof(ClickerModel.Data.GoldPerSecLevel));
    }
}
