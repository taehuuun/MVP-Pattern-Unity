using MethodType = ClickerModel.MethodType;

public class ClickerPresenter : PresenterBase<ClickerModel>
{
    protected override void Initialize()
    {
        base.Initialize();

        InvokeMethod(MethodType.StartGetGoldPerSec);
    }
}
