using UnityEngine;

/// <summary>
/// 클리커 게임 사용 Model 클래스, ModelBase 추상 클래스를 상속
/// </summary>
public class ClickerModel : ModelBase
{
    public PlayerData Data { get; private set; }                                                // 플레이어 데이터 (보유 골드, 각 업그레이드 레벨)
    public int CurrentGoldPerClick { get; private set; }                                        // 현재 클릭 당 획득 골드 량
    public int CurrentGoldPerSec { get; private set; }                                          // 현재 초 당 획득 골드 량
    public int NextGoldPerClickCost => _goldPerClickUpgrade.costs[Data.goldPerClickLevel+1];    // 다음 레벨의 클릭 당 골드 업그레이드 비용
    public int NextGoldPerSecCost => _goldPerSecUpgrade.costs[Data.goldPerSecLevel+1];          // 다음 레벨의 초 당 골드 업그레이드 비용
    public int NextGoldPerClickLevel => Data.goldPerClickLevel + 1;                             // 클릭 당 골드 업그레이드의 다음 레벨
    public int NextGoldPerSecLevel => Data.goldPerSecLevel + 1;                                 // 초 당 골드 업그레이드의 다음 레벨
    
    private Upgrade _goldPerClickUpgrade;                                                       // 클릭 당 골드 업그레이드 정보
    private Upgrade _goldPerSecUpgrade;                                                         // 초 당 골드 업그레이드 정보

    private const string _GOLD_PER_CLICK_LEVEL_KEY = "ClickerGoldPerClickLevel";                // 클릭 당 골드 업그레이드 레벨 저장을 위한 PlayerPrefs 키 값
    private const string _GOLD_PER_SEC_LEVEL_KEY = "ClickerGoldPerSecLevel";                    // 초 당 골드 업그레이드 레벨 저장을 위한 PlayerPrefs 키 값
    private const string _GOLD_KEY = "ClickerGold";                                             // 현재 보유 골드 저장을 위한 PlayerPrefs 키 값

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

    public void ClickAddGold()
    {
        Data.gold += CurrentGoldPerClick;
        TriggerEvent();
    }

    public void SecAddGold()
    {
        Data.gold += CurrentGoldPerSec;
        TriggerEvent();
    }

    public void UpgradeGoldPerClick()
    {
        int cost = _goldPerClickUpgrade.costs[NextGoldPerClickLevel];
        
        if (cost > Data.gold)
        {
            Debug.Log(cost);
            Debug.Log(NextGoldPerClickLevel);
            Debug.Log("클릭 당 골드 증가 업그레이드 비용 부족");
            return;
        }

        Data.gold -= cost;
        Data.goldPerClickLevel++;
        CurrentGoldPerClick = _goldPerClickUpgrade.values[Data.goldPerClickLevel];
        TriggerEvent();
    }
    
    public void UpgradeGoldPerSec()
    {
        int cost = _goldPerClickUpgrade.costs[NextGoldPerSecLevel];
        
        if (cost > Data.gold)
        {
            Debug.Log("초 당 골드 증가 업그레이드 비용 부족");
            return;
        }

        Data.gold -= cost;
        Data.goldPerSecLevel++;
        CurrentGoldPerSec = _goldPerSecUpgrade.values[Data.goldPerSecLevel];
        TriggerEvent();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_GOLD_KEY, Data.gold);
        PlayerPrefs.SetInt(_GOLD_PER_SEC_LEVEL_KEY, Data.goldPerSecLevel);
        PlayerPrefs.SetInt(_GOLD_PER_CLICK_LEVEL_KEY, Data.goldPerClickLevel);
    }
}
