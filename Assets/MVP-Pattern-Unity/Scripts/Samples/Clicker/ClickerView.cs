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