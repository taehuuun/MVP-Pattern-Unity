using TMPro;
using UnityEngine.UI;
using MethodType = ClickerModel.MethodType;

public class ClickerView : ViewBase
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

    protected override void InitializeBindComponent()
    {
        base.InitializeBindComponent();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    protected override void InitializeEventListeners()
    {
        base.InitializeEventListeners();
        GetBind<Button>((int)Buttons.TouchPanelBtn).onClick.AddListener(() => InvokeMethod(MethodType.ClickAddGold));
        GetBind<Button>((int)Buttons.ClickUpgradeBtn).onClick.AddListener(() => InvokeMethod(MethodType.UpgradeGoldPerClick));
        GetBind<Button>((int)Buttons.SecUpgradeBtn).onClick.AddListener(() => InvokeMethod(MethodType.UpgradeGoldPerSec));
    }

    protected override void UpdateView(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ClickerModel.Data.Gold):
                GetBind<TMP_Text>((int)Texts.CurrentGoldText).text = $"{GetProperty<int>(propertyName)} G";
                break;
            case nameof(ClickerModel.Data.GoldPerClickLevel):
                GetBind<TMP_Text>((int)Texts.ClickUpgradeLevelText).text = $"Next Lv: {GetProperty<int>(nameof(ClickerModel.Data.GoldPerClickLevel))}";
                break;
            case nameof(ClickerModel.CurrentGoldPerClick):
                GetBind<TMP_Text>((int)Texts.GoldPerClickText).text = $"+{GetProperty<int>(nameof(ClickerModel.CurrentGoldPerClick))} / Click";
                break;
            case nameof(ClickerModel.NextGoldPerClickCost):
                GetBind<TMP_Text>((int)Texts.ClickUpgradeCostText).text = $"Next Cost: {GetProperty<int>(nameof(ClickerModel.NextGoldPerClickCost))}";
                break;
            case nameof(ClickerModel.CurrentGoldPerSec):
                GetBind<TMP_Text>((int)Texts.GoldPerSecText).text = $"+{GetProperty<int>(nameof(ClickerModel.CurrentGoldPerSec))} / Sec";
                break;
            case nameof(ClickerModel.Data.GoldPerSecLevel):
                GetBind<TMP_Text>((int)Texts.SecUpgradeLevelText).text = $"Next Lv: {GetProperty<int>(nameof(ClickerModel.Data.GoldPerSecLevel))}";
                break;
            case nameof(ClickerModel.NextGoldPerSecCost):
                GetBind<TMP_Text>((int)Texts.SecUpgradeCostText).text = $"Next Cost: {GetProperty<int>(nameof(ClickerModel.NextGoldPerSecCost))}";
                break;
        }
    }
}