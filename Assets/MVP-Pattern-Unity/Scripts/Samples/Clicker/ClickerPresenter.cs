using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);     // 1초 딜레이
    
    /// <inheritdoc cref="PresenterBase.Initialize"/>
    protected override void Initialize()
    {
        base.Initialize();

        AddListener("TouchScreen", HandleTouchScreen);
        AddListener("UpgradeGoldPerClick", HandleUpgradePerClick);
        AddListener("UpgradeGoldPerSec", HandleUpgradePerSec);
        
        StartCoroutine(GetGoldPerSec());
    }
    
    /// <summary>
    /// 클릭 시 골드 증가 이벤트 핸틀링 메서드
    /// </summary>
    private void HandleTouchScreen()
    {
        ((ClickerModel)model).ClickAddGold();
    }
    
    /// <summary>
    /// 클릭 당 골드 증가량 업그레이드 이벤트 핸들링 메서드
    /// </summary>
    private void HandleUpgradePerClick()
    {
        ((ClickerModel)model).UpgradeGoldPerClick();
    }
    
    /// <summary>
    /// 초 당 골드 증가량 업그레이드 이벤트 핸들링 메서드
    /// </summary>
    private void HandleUpgradePerSec()
    {
        ((ClickerModel)model).UpgradeGoldPerSec();
    }
    
    /// <summary>
    /// 매 초 마다 골드를 증가 시키는 코루틴 메서드
    /// </summary>
    private IEnumerator GetGoldPerSec()
    {
        while (true)
        {
            ((ClickerModel)model).SecAddGold();
            yield return _goldPerSecDelay;
        }
    }
}
