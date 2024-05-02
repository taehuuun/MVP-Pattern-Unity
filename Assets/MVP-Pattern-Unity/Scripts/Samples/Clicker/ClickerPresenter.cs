using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);
    
    public override void Initialize()
    {
        base.Initialize();

        StartCoroutine(GetGoldPerSec());
    }
    
    public override void AddViewListeners()
    {
        base.AddViewListeners();
        view.AddListener("TouchScreen", HandleTouchScreen);
        view.AddListener("UpgradeGoldPerClick", HandleUpgradePerClick);
        view.AddListener("UpgradeGoldPerSec", HandleUpgradePerSec);
    }
    
    private void HandleTouchScreen()
    {
        ((ClickerModel)model).ClickAddGold();
    }
    
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
