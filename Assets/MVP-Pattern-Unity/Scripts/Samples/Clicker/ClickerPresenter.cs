using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);     // 1초 딜레이
    
    /// <inheritdoc cref="PresenterBase.Initialize"/>
    public override void Initialize()
    {
        base.Initialize();

        StartCoroutine(GetGoldPerSec());
    }
    
    /// <inheritdoc cref="PresenterBase.AddViewListeners"/>
    public override void AddViewListeners()
    {
        base.AddViewListeners();
        view.AddListener("TouchScreen", HandleTouchScreen);
        view.AddListener("UpgradeGoldPerClick", HandleUpgradePerClick);
        view.AddListener("UpgradeGoldPerSec", HandleUpgradePerSec);
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
    
    private void HandleUpgradePerSec()
    {
        ((ClickerModel)model).UpgradeGoldPerSec();
    }
    
    private IEnumerator GetGoldPerSec()
    {
        while (true)
        {
            ((ClickerModel)model).SecAddGold();
            yield return _goldPerSecDelay;
        }
    }
}
