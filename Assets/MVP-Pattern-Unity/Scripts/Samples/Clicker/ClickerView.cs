using TMPro;
using UnityEngine.UI;

public class ClickerView : ViewBase<ClickerModel>
{
    private enum Texts
    {
        GoldPerClickText,
        CurrentGoldText,
        GoldPerSecText,
        ClickUpgradeLevelText,
        ClickUpgradeCostText,
        SecUpgradeLevelText,
        SecUpgradeCostText
    }

    private enum Buttons
    {
        TouchPanelBtn,
        ClickUpgradeBtn,
        SecUpgradeBtn
    }
    
    protected override void InitializeBind()
    {
        base.InitializeBind();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    protected override void InitializeEvents(PresenterBase<ClickerModel> presenter)
    {
        base.InitializeEvents(presenter);
        GetBind<Button>((int)Buttons.TouchPanelBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.ClickAddGold));
        GetBind<Button>((int)Buttons.ClickUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerClick));
        GetBind<Button>((int)Buttons.SecUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.MethodType.UpgradeGoldPerSec));
    }

    public override void UpdateView(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ClickerModel.Data.Gold):
                GetBind<TMP_Text>((int)Texts.CurrentGoldText).text = $"{GetModel().Data.Gold} G";
                break;
            case nameof(ClickerModel.Data.GoldPerClickLevel):
                UpdateGoldPerClickUpdateUI();
                break;
            case nameof(ClickerModel.Data.GoldPerSecLevel):
                UpdateGoldPerSecUpdateUI();
                break;
        }
    }

    private void UpdateGoldPerClickUpdateUI()
    {
        GetBind<TMP_Text>((int)Texts.GoldPerClickText).text = $"+{GetModel().CurrentGoldPerClick} / Click";
        GetBind<TMP_Text>((int)Texts.ClickUpgradeLevelText).text = $"Next Lv: {GetModel().Data.GoldPerClickLevel}";
        GetBind<TMP_Text>((int)Texts.ClickUpgradeCostText).text = $"Next Cost: {GetModel().NextGoldPerClickCost}";
    }

    private void UpdateGoldPerSecUpdateUI()
    {
        GetBind<TMP_Text>((int)Texts.GoldPerSecText).text = $"+{GetModel().CurrentGoldPerSec} / Sec";
        GetBind<TMP_Text>((int)Texts.SecUpgradeLevelText).text = $"Next Lv: {GetModel().Data.GoldPerSecLevel}";
        GetBind<TMP_Text>((int)Texts.SecUpgradeCostText).text = $"Next Cost: {GetModel().NextGoldPerSecCost}";
    }
}