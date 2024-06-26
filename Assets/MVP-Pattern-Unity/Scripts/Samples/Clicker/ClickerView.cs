using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerView : ViewBase<ClickerModel>
{
    [Header("Header UI")]
    [SerializeField] private TMP_Text goldPerClickText;                         // 현재 클릭 당 골드 증가량 텍스트
    [SerializeField] private TMP_Text goldPerSecText;                           // 현재 초 당 골드 증가량 텍스트
    [SerializeField] private TMP_Text currentGoldText;                          // 현재 보유 골드 텍스트
    
    [Header("Upgrade UI")]
    [SerializeField] private Button goldPerClickUpgradeBtn;                     // 클릭 당 골드 증가량 업그레이드 버튼
    [SerializeField] private Button goldPerSecUpgradeBtn;                       // 초 당 골드 증가량 업그레이드 버튼
    [SerializeField] private TMP_Text goldPerClickNextLevelText;                // 다음 클릭 당 골드 증가량 업그레이드 레벨 텍스트 
    [SerializeField] private TMP_Text goldPerSecNextLevelText;                  // 다음 초 당 골드 증가량 업그레이드 레벨 텍스트
    [SerializeField] private TMP_Text goldPerClickNextCostText;                 // 다음 클릭 당 골드 증가량 업그레이드 비용 텍스트
    [SerializeField] private TMP_Text goldPerSecNextCostText;                   // 다음 초 당 골드 증가량 업그레이드 비용 텍스트

    [Header("Touch UI")]
    [SerializeField] private Button screenBtn;                                  // 골드 수집 가능한 화면 스크린 버튼
    
    [Header("Circle UI")]
    [SerializeField] private Transform circle;                                  // 화면 중앙의 원 트랜스폼

    private readonly float _scaleUpTime = 0.1f;                                 // 원의 사이즈 업이 걸리는 시간
    private bool _isSizeUpStart;
    
    /// <inheritdoc cref="ViewBase{TModel}.Initialize"/>
    public override void Initialize(PresenterBase<ClickerModel> presenter)
    {
        base.Initialize(presenter);
        
        screenBtn.onClick.AddListener(() => presenter.TriggerEvent("TouchScreen"));
        goldPerClickUpgradeBtn.onClick.AddListener(() => presenter.TriggerEvent("UpgradeGoldPerClick"));
        goldPerSecUpgradeBtn.onClick.AddListener(() => presenter.TriggerEvent("UpgradeGoldPerSec"));
    }

    /// <inheritdoc cref="ViewBase{TModel}.UpdateView"/>
    public override void UpdateView(ClickerModel changedModel)
    {
        goldPerClickText.text = $"+{changedModel.CurrentGoldPerClick} / Click";
        goldPerSecText.text = $"+{changedModel.CurrentGoldPerSec} / Sec";
        currentGoldText.text = $"{changedModel.Data.gold} G";
        goldPerClickNextLevelText.text = $"Next Lv: {changedModel.NextGoldPerClickLevel}";
        goldPerSecNextLevelText.text = $"Next Lv: {changedModel.NextGoldPerSecLevel}";
        goldPerClickNextCostText.text = $"Cost: {changedModel.NextGoldPerClickCost} G";
        goldPerSecNextCostText.text = $"Cost: {changedModel.NextGoldPerSecCost} G";

        if (!_isSizeUpStart)
        {
            StartCoroutine(SizeUp(changedModel.Data.gold));
        }
    }

    /// <summary>
    /// 골드 증가에 따라 화면 중앙 원 사이즈를 키우는 코루틴 메서드
    /// </summary>
    /// <param name="gold">증가된 골드</param>
    private IEnumerator SizeUp(int gold)
    {
        _isSizeUpStart = true;
        float targetScaleValue = Mathf.Clamp(gold * 0.00001f,0.01f, 6.5f);
        Vector2 targetScale = new Vector2(targetScaleValue, targetScaleValue);
        Vector2 originScale = circle.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < _scaleUpTime)
        {
            circle.localScale = Vector2.Lerp(originScale, targetScale, elapsedTime / _scaleUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        circle.localScale = targetScale;
        _isSizeUpStart = false;
    }
}