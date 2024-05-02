using UnityEngine;

public class ClickerModel : ModelBase
{
    public PlayerData Data { get; private set; }
    public int CurrentGoldPerClick { get; private set; }
    public int CurrentGoldPerSec { get; private set; }
    public int NextGoldPerClickCost => _goldPerClickUpgrade.costs[Data.goldPerClickLevel+1];
    public int NextGoldPerSecCost => _goldPerSecUpgrade.costs[Data.goldPerSecLevel+1];
    public int NextGoldPerClickLevel => Data.goldPerClickLevel + 1;
    public int NextGoldPerSecLevel => Data.goldPerSecLevel + 1;
    
    private Upgrade _goldPerClickUpgrade;
    private Upgrade _goldPerSecUpgrade;

    private const string _GOLD_PER_CLICK_LEVEL_KEY = "ClickerGoldPerClickLevel";
    private const string _GOLD_PER_SEC_LEVEL_KEY = "ClickerGoldPerSecLevel";
    private const string _GOLD_KEY = "ClickerGold";

    public override void Initialize()
    {
        base.Initialize();

        Upgrade goldPerClick = Resources.Load<Upgrade>("Clicker/SO/GoldPerClick");
        Upgrade goldPerSec = Resources.Load<Upgrade>("Clicker/SO/GoldPerSec");

        _goldPerClickUpgrade = goldPerClick;
        _goldPerSecUpgrade = goldPerSec;

        Data = new ()
        {
            gold = PlayerPrefs.GetInt(_GOLD_KEY,0),
            goldPerClickLevel = PlayerPrefs.GetInt(_GOLD_PER_CLICK_LEVEL_KEY, 0),
            goldPerSecLevel = PlayerPrefs.GetInt(_GOLD_PER_SEC_LEVEL_KEY,0)
        };
        
        CurrentGoldPerClick = _goldPerClickUpgrade.values[Data.goldPerClickLevel];
        CurrentGoldPerSec = _goldPerSecUpgrade.values[Data.goldPerSecLevel];
    }
}
