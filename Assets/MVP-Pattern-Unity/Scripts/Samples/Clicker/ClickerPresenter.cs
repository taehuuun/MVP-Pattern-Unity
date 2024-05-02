using System.Collections;
using UnityEngine;

public class ClickerPresenter : PresenterBase
{
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);
    
    private IEnumerator GetGoldPerSec()
    {
        while (true)
        {
            ((ClickerModel)model).SecAddGold();
            yield return _goldPerSecDelay;
        }
    }
}
