using System.Collections;
using UnityEngine;

/// <summary>
/// 클리커 게임 사용 Model 클래스, ModelBase 추상 클래스를 상속
/// </summary>
public class ClickerModel : ModelBase
{
    public enum MethodType
    {
        ClickAddGold,
        SecAddGold,
        UpgradeGoldPerClick,
        UpgradeGoldPerSec,
        StartGetGoldPerSec
    }
    
    [SerializeField] private Transform circle;                                  // 화면 중앙의 원 트랜스폼

    public PlayerData Data { get; private set; } = new();

    public int CurrentGoldPerClick => _goldPerClickUpgrade.values[Data.GoldPerClickLevel];
    public int CurrentGoldPerSec => _goldPerSecUpgrade.values[Data.GoldPerSecLevel];
    public int NextGoldPerClickCost => _goldPerClickUpgrade.costs[Data.GoldPerClickLevel+1];    // 다음 레벨의 클릭 당 골드 업그레이드 비용
    public int NextGoldPerSecCost => _goldPerSecUpgrade.costs[Data.GoldPerSecLevel+1];          // 다음 레벨의 초 당 골드 업그레이드 비용

    private Upgrade _goldPerClickUpgrade;                                                       // 클릭 당 골드 업그레이드 정보
    private Upgrade _goldPerSecUpgrade;                                                         // 초 당 골드 업그레이드 정보
    
    private const string _GOLD_PER_CLICK_LEVEL_KEY = "ClickerGoldPerClickLevel";                // 클릭 당 골드 업그레이드 레벨 저장을 위한 PlayerPrefs 키 값
    private const string _GOLD_PER_SEC_LEVEL_KEY = "ClickerGoldPerSecLevel";                    // 초 당 골드 업그레이드 레벨 저장을 위한 PlayerPrefs 키 값
    private const string _GOLD_KEY = "ClickerGold";                                             // 현재 보유 골드 저장을 위한 PlayerPrefs 키 값
    
    private int _currentGoldPerClick;
    private int _currentGoldPerSec;
    private readonly float _scaleUpTime = 0.1f;                                                 // 원의 사이즈 업이 걸리는 시간
    private readonly WaitForSeconds _goldPerSecDelay = new(1f);                                 // 1초 딜레이

    protected override void InitializeMethods()
    {
        base.InitializeMethods();
        
        AddMethod(MethodType.ClickAddGold, ClickAddGold);
        AddMethod(MethodType.SecAddGold, SecAddGold);
        AddMethod(MethodType.UpgradeGoldPerClick, UpgradeGoldPerClick);
        AddMethod(MethodType.UpgradeGoldPerSec, UpgradeGoldPerSec);
        AddMethod(MethodType.StartGetGoldPerSec, StartGetGoldPerSec);
    }
    protected override void InitializeNestedProperties()
    {
        base.InitializeNestedProperties();
        
        Data.PropertyChanged += OnNestedPropertyChanged;
    }
    protected override void InitializeProperties()
    {
        _goldPerClickUpgrade = Resources.Load<Upgrade>("Clicker/SO/GoldPerClick");
        _goldPerSecUpgrade = Resources.Load<Upgrade>("Clicker/SO/GoldPerSec");

        Data.Gold = PlayerPrefs.GetInt(_GOLD_KEY,0);
        Data.GoldPerClickLevel = PlayerPrefs.GetInt(_GOLD_PER_CLICK_LEVEL_KEY, 0);
        Data.GoldPerSecLevel = PlayerPrefs.GetInt(_GOLD_PER_SEC_LEVEL_KEY, 0);
    }

    private void StartGetGoldPerSec() => StartCoroutine(GetGoldPerSec()); 
    
    /// <summary>
    /// 클릭 당 골드 증가량 만큼 골드를 증가 시키는 메서드 
    /// </summary>
    private void ClickAddGold()
    {
        Data.Gold += CurrentGoldPerClick;
        StartCoroutine(SizeUp(Data.Gold));
    }

    /// <summary>
    /// 초 당 골드 증가량 만큼 골드를 증가 시키는 메서드 
    /// </summary>
    private void SecAddGold()
    {
        Data.Gold += CurrentGoldPerSec;
    }

    /// <summary>
    /// 클릭 당 골드 증가량 업그레이드 메서드
    /// </summary>
    private void UpgradeGoldPerClick()
    {
        if (NextGoldPerClickCost > Data.Gold)
        {
            Debug.Log("클릭 당 골드 증가 업그레이드 비용 부족");
            return;
        }

        Data.Gold -= NextGoldPerClickCost;
        Data.GoldPerClickLevel++;
        StartCoroutine(SizeUp(Data.Gold));
    }
    
    /// <summary>
    /// 초 당 골드 증가량 업그레이드 메서드
    /// </summary>
    private void UpgradeGoldPerSec()
    {
        if (NextGoldPerSecCost > Data.Gold)
        {
            Debug.Log("초 당 골드 증가 업그레이드 비용 부족");
            return;
        }

        Data.Gold -= NextGoldPerSecCost;
        Data.GoldPerSecLevel++;
        StartCoroutine(SizeUp(Data.Gold));
    }

    /// <summary>
    /// 게임 종료 시 진행 내용을 PlayerPrefs에 저장하는 메서드
    /// </summary>
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_GOLD_KEY, Data.Gold);
        PlayerPrefs.SetInt(_GOLD_PER_SEC_LEVEL_KEY, Data.GoldPerSecLevel);
        PlayerPrefs.SetInt(_GOLD_PER_CLICK_LEVEL_KEY, Data.GoldPerClickLevel);
    }
    
    /// <summary>
    /// 골드 증가에 따라 화면 중앙 원 사이즈를 키우는 코루틴 메서드
    /// </summary>
    /// <param name="gold">증가된 골드</param>
    private IEnumerator SizeUp(int gold)
    {
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
    }
    
    private IEnumerator GetGoldPerSec()
    {
        while (true)
        {
            SecAddGold();
            yield return StartCoroutine(SizeUp(Data.Gold));
            yield return _goldPerSecDelay;
        }
    }
}
