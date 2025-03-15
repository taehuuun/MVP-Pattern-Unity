public class PlayerData : BindableData
{
    public int Gold
    {
        get => _gold;
        set => SetProperty(ref _gold, value);
    }

    public int GoldPerClickLevel
    {
        get => _goldPerClickLevel;
        set => SetProperty(ref _goldPerClickLevel, value);
    }

    public int GoldPerSecLevel
    {
        get => _goldPerSecLevel;
        set => SetProperty(ref _goldPerSecLevel, value);
    }
    
    private int _gold = int.MinValue;
    private int _goldPerClickLevel = int.MinValue;
    private int _goldPerSecLevel = int.MinValue;
}
