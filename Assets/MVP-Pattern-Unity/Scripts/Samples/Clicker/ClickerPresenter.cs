using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);
    
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
