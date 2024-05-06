using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase<ClickerModel>
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);     // 1초 딜레이
    
    /// <inheritdoc cref="PresenterBase{TModel}.Initialize"/>
    protected override void Initialize()
    {
        base.Initialize();

        AddListener("TouchScreen", model.ClickAddGold);
        AddListener("UpgradeGoldPerClick", model.UpgradeGoldPerClick);
        AddListener("UpgradeGoldPerSec", model.UpgradeGoldPerSec);
        
        StartCoroutine(GetGoldPerSec());
    }
    
    /// <summary>
    /// 매 초 마다 골드를 증가 시키는 코루틴 메서드
    /// </summary>
    private IEnumerator GetGoldPerSec()
    {
        while (true)
        {
            model.SecAddGold();
            yield return _goldPerSecDelay;
        }
    }
}
