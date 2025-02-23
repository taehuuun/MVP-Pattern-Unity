using TMPro;
using UnityEngine.UI;

public class ClickerView : ViewBase<ClickerModel>
{
    private enum Texts
    {
        GoldPerClickText,
        CurrentGoldText,
        GoldPerSecText,
        ClickUpgradeTitleText,
        ClickUpgradeLevelText,
        ClickUpgradeCostText,
        SecUpgradeTitleText,
        SecUpgradeLevelText,
        SecUpgradeCostText
    }

    private enum Buttons
    {
        TouchPanelBtn,
        ClickUpgradeBtn,
        SecUpgradeBtn
    }

    /// <inheritdoc cref="ViewBase{TModel}.Initialize"/>
    public override void Initialize(PresenterBase<ClickerModel> presenter)
    {
        base.Initialize(presenter);
        
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetBindUI<Button>((int)Buttons.TouchPanelBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.Method.ClickAddGold));
        GetBindUI<Button>((int)Buttons.ClickUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.Method.UpgradeGoldPerClick));
        GetBindUI<Button>((int)Buttons.SecUpgradeBtn).onClick.AddListener(() => presenter.InvokeMethod(ClickerModel.Method.UpgradeGoldPerSec));
    }

    public override void UpdateView(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ClickerModel.Data.Gold):
                GetBindUI<TMP_Text>((int)Texts.CurrentGoldText).text = $"{GetModel().Data.Gold} G";
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
        GetBindUI<TMP_Text>((int)Texts.GoldPerClickText).text = $"+{GetModel().CurrentGoldPerClick} / Click";
        GetBindUI<TMP_Text>((int)Texts.ClickUpgradeLevelText).text = $"Next Lv: {GetModel().Data.GoldPerClickLevel}";
        GetBindUI<TMP_Text>((int)Texts.ClickUpgradeCostText).text = $"Next Cost: {GetModel().NextGoldPerClickCost}";
    }

    private void UpdateGoldPerSecUpdateUI()
    {
        GetBindUI<TMP_Text>((int)Texts.GoldPerSecText).text = $"+{GetModel().CurrentGoldPerSec} / Sec";
        GetBindUI<TMP_Text>((int)Texts.SecUpgradeLevelText).text = $"Next Lv: {GetModel().Data.GoldPerSecLevel}";
        GetBindUI<TMP_Text>((int)Texts.SecUpgradeCostText).text = $"Next Cost: {GetModel().NextGoldPerSecCost}";
    }
}