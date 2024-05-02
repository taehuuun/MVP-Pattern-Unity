using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerView : ViewBase
{
    [Header("Header UI")]
    [SerializeField] private TMP_Text goldPerClickText;
    [SerializeField] private TMP_Text goldPerSecText;
    [SerializeField] private TMP_Text currentGoldText;
    
    [Header("Upgrade UI")]
    [SerializeField] private Button goldPerClickUpgradeBtn;
    [SerializeField] private Button goldPerSecUpgradeBtn;
    [SerializeField] private TMP_Text nextGoldPerClickUpgradeBtnLevelText;
    [SerializeField] private TMP_Text nextGoldPerSecUpgradeBtnLevelText;
    [SerializeField] private TMP_Text nextGoldPerClickUpgradeBtnCostText;
    [SerializeField] private TMP_Text nextGoldPerSecUpgradeBtnCostText;

    [Header("Touch UI")]
    [SerializeField] private Button screenBtn;
    
    [Header("Circle UI")]
    [SerializeField] private Transform circle;

    private readonly float _scaleUpTime = 0.2f;

    public override void UpdateView(ModelBase model)
    {

    }
}