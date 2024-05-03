using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerView : ViewBase
{
    [Header("Header UI")]
    [SerializeField] private TMP_Text goldPerClickText;                         // 현재 클릭 당 골드 증가량 텍스트
    [SerializeField] private TMP_Text goldPerSecText;                           // 현재 초 당 골드 증가량 텍스트
    [SerializeField] private TMP_Text currentGoldText;                          // 현재 보유 골드 텍스트
    
    [Header("Upgrade UI")]
    [SerializeField] private Button goldPerClickUpgradeBtn;                     // 클릭 당 골드 증가량 업그레이드 버튼
    [SerializeField] private Button goldPerSecUpgradeBtn;                       // 초 당 골드 증가량 업그레이드 버튼
    [SerializeField] private TMP_Text nextGoldPerClickUpgradeBtnLevelText;      // 다음 클릭 당 골드 증가량 업그레이드 레벨 텍스트 
    [SerializeField] private TMP_Text nextGoldPerSecUpgradeBtnLevelText;        // 다음 초 당 골드 증가량 업그레이드 레벨 텍스트
    [SerializeField] private TMP_Text nextGoldPerClickUpgradeBtnCostText;       // 다음 클릭 당 골드 증가량 업그레이드 비용 텍스트
    [SerializeField] private TMP_Text nextGoldPerSecUpgradeBtnCostText;         // 다음 초 당 골드 증가량 업그레이드 비용 텍스트

    [Header("Touch UI")]
    [SerializeField] private Button screenBtn;                                  // 골드 수집 가능한 화면 스크린 버튼
    
    [Header("Circle UI")]
    [SerializeField] private Transform circle;                                  // 화면 중앙의 원 트랜스폼

    private readonly float _scaleUpTime = 0.2f;                                 // 원의 사이즈 업이 걸리는 시간
    
    /// <inheritdoc cref="ViewBase.Initialize"/>
    public override void Initialize()
    {
        screenBtn.onClick.AddListener(TriggerTouchScreen);
        goldPerClickUpgradeBtn.onClick.AddListener(TriggerGoldPerClickUpgrade);
        goldPerSecUpgradeBtn.onClick.AddListener(TriggerGoldPerSecUpgrade);
    }

    /// <inheritdoc cref="ViewBase.UpdateView"/>
    public override void UpdateView(ModelBase changedModel)
    {
        ClickerModel clickerModel = changedModel as ClickerModel;
        goldPerClickText.text = $"+{clickerModel.CurrentGoldPerClick} / Click";
        goldPerSecText.text = $"+{clickerModel.CurrentGoldPerSec} / Sec";
        currentGoldText.text = $"{clickerModel.Data.gold} G";
        nextGoldPerClickUpgradeBtnLevelText.text = $"Next Lv: {clickerModel.NextGoldPerClickLevel}";
        nextGoldPerSecUpgradeBtnLevelText.text = $"Next Lv: {clickerModel.NextGoldPerSecLevel}";
        nextGoldPerClickUpgradeBtnCostText.text = $"Cost: {clickerModel.NextGoldPerClickCost} G";
        nextGoldPerSecUpgradeBtnCostText.text = $"Cost: {clickerModel.NextGoldPerSecCost} G";

        StartCoroutine(SizeUp(clickerModel.Data.gold));
    }

    /// <summary>
    /// 클릭 시 골드 증가 이벤트를 트리거 하는 메서드
    /// </summary>
    private void TriggerTouchScreen()
    {
        TriggerEvent("TouchScreen");
    }

    /// <summary>
    /// 클릭 당 골드 증가량 업그레이드 이벤트를 트리거 하는 메서드
    /// </summary>
    private void TriggerGoldPerClickUpgrade()
    {
        TriggerEvent("UpgradeGoldPerClick");
    }

    private void TriggerGoldPerSecUpgrade()
    {
        TriggerEvent("UpgradeGoldPerSec");
    }

    private IEnumerator SizeUp(int gold)
    {
        float targetScaleValue = circle.localScale.x + gold * 0.001f;
        Vector2 targetScale = new Vector2(targetScaleValue, targetScaleValue);
        Vector2 originScale = circle.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < _scaleUpTime)
        {
            transform.localScale = Vector2.Lerp(originScale, targetScale, elapsedTime / _scaleUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}